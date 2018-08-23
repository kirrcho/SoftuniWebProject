using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyAnimeWorld.Common.Animes.ViewModels;
using MyAnimeWorld.Common.Utilities.Censor;
using MyAnimeWorld.Data;
using MyAnimeWorld.Models;

namespace MyAnimeWorld.Services
{
    public class EpisodeService : BaseService
    {
        public EpisodeService(AnimeWorldContext animeWorldContext, IMapper mapper)
           : base(animeWorldContext, mapper)
        {
        }

        public async Task<AnimeEpisode> FindAsync(int id) => await this.AnimeContext.Episodes.FindAsync(id);

        public async Task<int> FindLastEpisodeNumberAsync(int animeSeriesId)
        {
            var lastEpisode = await this.AnimeContext.Episodes
                .Where(p => p.AnimeSeriesId == animeSeriesId)
                .OrderBy(p => p.EpisodeNumber)
                .LastOrDefaultAsync();

            //If there are no episodes return 0
            return lastEpisode?.EpisodeNumber ?? 0;
        }

        public async Task<List<AnimeLinkEnum>> GetAllSourceLinksAsync()
        {
            List<AnimeLinkEnum> animeLinks = await this.AnimeContext.AnimeSourceLinks.OrderBy(p => p.Id).ToListAsync();
            //For easier recognition of the links when admin inputs some or all of them (Check the page for more info)
            return animeLinks;
        }

        public async Task<List<AnimeLink>> GetAllLinksForAsync(int episodeId)
        {
            var links = await this.AnimeContext.AnimeLinks
                .Where(p => p.EpisodeId == episodeId)
                .Include(p => p.Source)
                .ToListAsync();

            //For easier recognition of the links when admin inputs some or all of them (Check the page for more info)
            return links;
        }

        public async Task<IEnumerable<AlternativeLinkViewModel>> GetAnimeLinksForAsync(int episodeId,int linkSelected)
        {
            var links = await this.AnimeContext.AnimeLinks.Where(p => p.EpisodeId == episodeId).Include(p => p.Source).ToListAsync();

            var viewModel = links.Select(p => new AlternativeLinkViewModel()
            {
                LinkId = p.Id,
                LinkName = p.Source.Name,
                Source = p.SourceUrl,
                LinkSelected = p.Source.Id == linkSelected ? true : false,
                EpisodeId = episodeId
            });

            return viewModel;
        }

        public async Task<bool> SourceLinkExistsAsync(string name)
        {
            var sourceLink = await this.AnimeContext.AnimeSourceLinks.FirstOrDefaultAsync(p => p.Name == name);
            return sourceLink != null;
        }

        public async Task<AnimeLink> GetLinkByNameAsync(string source)
        {
            var link = await this.AnimeContext.AnimeLinks
                .Include(p => p.Source)
                .FirstOrDefaultAsync(p => p.Source.Name == source);

            return link;
        }

        public async Task AddAnimeLinkEnumAsync(AnimeLinkEnum linkEnum)
        {
            await this.AnimeContext.AnimeSourceLinks.AddAsync(linkEnum);
            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task AddSourceLinkToEpisodeAsync(int animeSeriesId, int animeLinkEnumId, string url, int episodeNumber)
        {
            var episode = await this.FindOrCreateEpisodeAsync(animeSeriesId, episodeNumber);
            await TryCreateAnimeLinkAsync(animeSeriesId, animeLinkEnumId, url, episode.Id);
        }

        public async Task AddSourceLinkAsync(AnimeLink link)
        {
            await this.AnimeContext.AnimeLinks.AddAsync(link);
            await this.AnimeContext.SaveChangesAsync();
        }

        private async Task TryCreateAnimeLinkAsync(int animeSeriesId, int animeLinkEnumId, string url, int episodeId)
        {
            if (await this.AnimeLinkExistsAsync(animeSeriesId, animeLinkEnumId, episodeId))
            {
                return;
            }

            var animeLink = new AnimeLink()
            {
                AnimeId = animeSeriesId,
                SourceId = animeLinkEnumId,
                SourceUrl = url,
                EpisodeId = episodeId
            };

            await this.AnimeContext.AnimeLinks.AddAsync(animeLink);
            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task<AnimeEpisode> FindOrCreateEpisodeAsync(int animeSeriesId, int episodeNumber)
        {
            var episode = await this.AnimeContext.Episodes
                .FirstOrDefaultAsync(p => p.AnimeSeriesId == animeSeriesId && p.EpisodeNumber == episodeNumber);

            if (episode == null)
            {
                episode = new AnimeEpisode()
                {
                    AnimeSeriesId = animeSeriesId,
                    EpisodeNumber = episodeNumber,
                    DateCreatedAt = DateTime.Now
                };

                await this.AnimeContext.Episodes.AddAsync(episode);
                await this.AnimeContext.SaveChangesAsync();
            }

            return episode;
        }

        public async Task<AnimeEpisode> FindEpisodeAsync(int animeSeriesId, int episodeNumber)
        {
            var ep = await this.AnimeContext.Episodes.FirstOrDefaultAsync(p => p.AnimeSeriesId == animeSeriesId && p.EpisodeNumber == episodeNumber);
            return ep;
        }

        public async Task<Dictionary<int, string>> GetAllEpisodeLinksAsync(int animeSeriesId, int episodeNumber)
        {
            var episode = await this.AnimeContext.Episodes
                .FirstOrDefaultAsync(p => p.AnimeSeriesId == animeSeriesId && p.EpisodeNumber == episodeNumber);

            if (episode == null)
            {
                return new Dictionary<int, string>();
            }

            var linkIds = this.AnimeContext.AnimeLinks
                .Where(p => p.EpisodeId == episode.Id && p.AnimeId == animeSeriesId)
                .ToDictionary(p => p.SourceId, k => k.SourceUrl);

            return linkIds;
        }

        public async Task<bool> AnimeEpisodeExistsAsync(int animeSeriesId, int episodeNumber)
        {
            var episode = await this.AnimeContext.Episodes.FirstOrDefaultAsync(p => p.AnimeSeriesId == animeSeriesId && p.EpisodeNumber == episodeNumber);

            return episode != null;
        }

        public async Task RemoveEpisodeSourceLinkAsync(int episodeId, int sourceId)
        {
            var animeLink = await this.AnimeContext.AnimeLinks.FirstOrDefaultAsync(p => p.EpisodeId == episodeId && p.SourceId == sourceId);
            this.AnimeContext.AnimeLinks.Remove(animeLink);
            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task<Dictionary<int, int>> GetAllEpisodesIdsAsync(int animeSeriesId)
        {
            var episodes = await this.AnimeContext.Episodes
                .Where(p => p.AnimeSeriesId == animeSeriesId)
                .ToDictionaryAsync(p => p.EpisodeNumber, k => k.Id);

            return episodes;
        }

        public async Task<bool> AnimeLinkExistsAsync(int animeSeriesId, int animeLinkEnumId, int episodeId)
        {
            var animeLink = await this.AnimeContext.AnimeLinks
                .FirstOrDefaultAsync(p => p.AnimeId == animeSeriesId && p.EpisodeId == episodeId && p.SourceId == animeLinkEnumId);
            return animeLink != null;
        }

        public async Task<IEnumerable<CommentViewModel>> LoadComments(int episodeId)
        {
            var comments = await this.AnimeContext.Comments
                .Where(p => p.EpisodeId == episodeId)
                .Include(p => p.User)
                .OrderByDescending(p => p.DateCreatedAt)
                .ToListAsync();

            var viewModel = this.Mapper.Map<IEnumerable<CommentViewModel>>(comments);

            return viewModel;
        }

        public async Task CreateComment(string content, int episodeId, string userId)
        {
            Comment comment = new Comment()
            {
                UserId = userId,
                EpisodeId = episodeId,
                CommentContent = WordsFilter.CensorComment(content),
                DateCreatedAt = DateTime.Now
            };

            await this.AnimeContext.Comments.AddAsync(comment);
            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task DeleteEpisode(int episodeId)
        {
            var episode = await this.AnimeContext.Episodes
                .Include(p => p.Comments)
                .Include(p => p.Links)
                .FirstOrDefaultAsync(p => p.Id == episodeId);
            
            this.AnimeContext.Comments.RemoveRange(episode.Comments);
            this.AnimeContext.AnimeLinks.RemoveRange(episode.Links);
            this.AnimeContext.Remove(episode);
            this.AnimeContext.SaveChanges();
        }

        public async Task<int> GetAllEpisodesCount(int animeSeriesId)
        {
            var series = await this.AnimeContext.AnimeSeries
                .Include(p => p.Episodes)
                .FirstOrDefaultAsync(p => p.Id == animeSeriesId);

            return series.Episodes.Count;
        }

        public async Task<int> GetNextEpisodeId(int currentEpisodeNumber,int animeSeriesId)
        {
            var episodes = await this.AnimeContext.Episodes.Where(p => p.AnimeSeriesId == animeSeriesId).ToListAsync();

            var episode = episodes.FirstOrDefault(p => p.EpisodeNumber == currentEpisodeNumber + 1);

            return episode?.Id ?? 0;
        }

        public async Task<int> GetPreviousEpisodeId(int currentEpisodeNumber, int animeSeriesId)
        {
            var episodes = await this.AnimeContext.Episodes.Where(p => p.AnimeSeriesId == animeSeriesId).ToListAsync();

            var episode = episodes.FirstOrDefault(p => p.EpisodeNumber == currentEpisodeNumber - 1);

            return episode?.Id ?? 0;
        }
    }
}
