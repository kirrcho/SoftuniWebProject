using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAnimeWorld.Data;
using MyAnimeWorld.Models;
using MyAnimeWorld.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeWorld.Tests
{
    [TestClass]
    public class EpisodeServiceTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Context = InitializedObjects.GetContext();
            this.EpisodeService = new EpisodeService(Context, Mapper.Instance);
        }

        public AnimeWorldContext Context { get; set; }

        public EpisodeService EpisodeService { get; set; }

        [TestMethod]
        public async Task FindAsync_WithInvalidObject()
        {
            var animeSeries = this.Context.Episodes.ToList();

            var anime = this.Context.Episodes.LastOrDefault();

            var test = await this.EpisodeService.FindAsync((anime?.Id ?? 0) + 1);
            var test2 = await this.EpisodeService.FindAsync(0);
            var test3 = await this.EpisodeService.FindAsync(-4);

            Assert.IsNull(test);
            Assert.IsNull(test2);
            Assert.IsNull(test3);
        }

        [TestMethod]
        public void FindAsync_WithValidObject()
        {
            var animeSeries = this.Context.Episodes.ToList();

            var anime = this.Context.Episodes.LastOrDefault();

            Assert.IsNotNull(this.EpisodeService.FindAsync(anime?.Id ?? 0));
        }

        [TestMethod]
        public async Task FindLastEpisodeNumberAsync_WithValidObject()
        {
            var animeSeriesId = this.Context.AnimeSeries.LastOrDefault(p => p.Episodes.Any()).Id;

            var episodeNumber = this.Context.Episodes
                .Where(p => p.AnimeSeriesId == animeSeriesId)
                .OrderByDescending(p => p.EpisodeNumber)
                .FirstOrDefault().EpisodeNumber;

            var result = await this.EpisodeService.FindLastEpisodeNumberAsync(animeSeriesId);

            Assert.AreEqual(episodeNumber, result);

            var episode = new AnimeEpisode()
            {
                AnimeSeriesId = animeSeriesId,
                DateCreatedAt = DateTime.UtcNow,
                EpisodeNumber = episodeNumber + 1
            };

            await this.Context.Episodes.AddAsync(episode);
            await this.Context.SaveChangesAsync();

            episodeNumber = this.Context.Episodes
                .Where(p => p.AnimeSeriesId == animeSeriesId)
                .OrderByDescending(p => p.EpisodeNumber)
                .FirstOrDefault().EpisodeNumber;

            result = await this.EpisodeService.FindLastEpisodeNumberAsync(animeSeriesId);

            Assert.AreEqual(episodeNumber, result);
        }

        [TestMethod]
        public async Task GetAllSourceLinksAsync_WithValidObject()
        {
            var links = this.Context.AnimeSourceLinks.ToList();

            var result = (await this.EpisodeService.GetAllSourceLinksAsync()).ToList();

            CollectionAssert.AreEqual(links, result);

            var link = new AnimeLinkEnum()
            {
                Name = "A different name that someone might have heard of"
            };

            await this.Context.AnimeSourceLinks.AddAsync(link);
            await this.Context.SaveChangesAsync();

            links = this.Context.AnimeSourceLinks.ToList();

            result = (await this.EpisodeService.GetAllSourceLinksAsync()).ToList();

            CollectionAssert.AreEqual(links, result);
        }

        [TestMethod]
        public async Task GetAllLinksForAsync_WithValidObject()
        {
            var episodeId = this.Context.Episodes.FirstOrDefault().Id;

            var links = this.Context.AnimeLinks.Where(p => p.EpisodeId == episodeId).ToList();

            var result = (await this.EpisodeService.GetAllLinksForAsync(episodeId)).ToList();

            CollectionAssert.AreEqual(links, result);

            var link = new AnimeLink()
            {
                AnimeId = this.Context.AnimeSeries.LastOrDefault().Id,
                EpisodeId = episodeId,
                Source = new AnimeLinkEnum()
                {
                    Name = "Bla Bla blllllla"
                },
            };

            await this.Context.AnimeLinks.AddAsync(link);
            await this.Context.SaveChangesAsync();

            links = this.Context.AnimeLinks.Where(p => p.EpisodeId == episodeId).ToList();

            result = (await this.EpisodeService.GetAllLinksForAsync(episodeId)).ToList();

            CollectionAssert.AreEqual(links, result);
        }

        [TestMethod]
        public async Task SourceLinkExistsAsync_WithValidObject()
        {
            var link = this.Context.AnimeSourceLinks.FirstOrDefault();
            var link2 = this.Context.AnimeSourceLinks.LastOrDefault();

            var result = await this.EpisodeService.SourceLinkExistsAsync(link.Name);
            var result2 = await this.EpisodeService.SourceLinkExistsAsync(link2.Name);

            Assert.IsTrue(result);
            Assert.IsTrue(result2);
        }

        [TestMethod]
        public async Task GetLinkByNameAsync_WithValidObject()
        {
            var sourceName = "New Source";

            var link = new AnimeLink()
            {
                AnimeId = this.Context.AnimeSeries.LastOrDefault().Id,
                EpisodeId = 3,
                Source = new AnimeLinkEnum()
                {
                    Name = sourceName
                },
                SourceUrl = "ksdld"
            };

            this.Context.AnimeLinks.Add(link);
            this.Context.SaveChanges();

            var result = await this.EpisodeService.GetLinkByNameAsync(sourceName);

            Assert.AreEqual(link, result);
        }

        [TestMethod]
        public async Task GetLinkByNameAsync_WithInvalidObject()
        {
            var result = await this.EpisodeService.GetLinkByNameAsync(null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task FindOrCreateEpisodeAsync_WithValidObject()
        {
            var animeSeriesId = this.Context.AnimeSeries.LastOrDefault(p => p.Episodes.Any()).Id;

            var episode = this.Context.Episodes.FirstOrDefault();

            var episode3 = new AnimeEpisode()
            {
                AnimeSeriesId = this.Context.AnimeSeries.FirstOrDefault().Id,
                EpisodeNumber = 8
            };
            var episode4 = new AnimeEpisode()
            {
                AnimeSeriesId = this.Context.AnimeSeries.FirstOrDefault().Id,
                EpisodeNumber = 9
            };

            var result = await this.EpisodeService.FindOrCreateEpisodeAsync(episode.AnimeSeriesId, episode.EpisodeNumber);
            var result3 = await this.EpisodeService.FindOrCreateEpisodeAsync(episode3.AnimeSeriesId, episode3.EpisodeNumber);
            var result4 = await this.EpisodeService.FindOrCreateEpisodeAsync(episode4.AnimeSeriesId, episode4.EpisodeNumber);

            Assert.AreEqual(episode, result);
            Assert.AreEqual(episode3.AnimeSeriesId, result3.AnimeSeriesId);
            Assert.AreEqual(episode3.EpisodeNumber, result3.EpisodeNumber);
            Assert.AreEqual(episode4.AnimeSeriesId, result4.AnimeSeriesId);
            Assert.AreEqual(episode4.EpisodeNumber, result4.EpisodeNumber);
        }

        [TestMethod]
        public async Task RemoveEpisodeSourceLinkAsync_WithValidObject()
        {
            var episode = this.Context.Episodes.Where(p => p.Links.Any()).FirstOrDefault();

            var episodeId = episode.Id;

            var sourceId = episode.Links.FirstOrDefault().SourceId;

            await this.EpisodeService.RemoveEpisodeSourceLinkAsync(episodeId, sourceId);

            var result = this.Context.Episodes.FirstOrDefault(p => p.Id == episodeId && p.Links.Any(k => k.SourceId == sourceId));

            Assert.IsNull(result);
        }
    }
}
