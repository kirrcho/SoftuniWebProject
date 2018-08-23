using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAnimeWorld.Common.Animes.ViewModels;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Models;
using MyAnimeWorld.Services;

namespace MyAnimeWorld.App.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class AddEpisodeModel : PageModel
    {
        public AddEpisodeModel(AnimeService animeService, EpisodeService episodeService, IMapper mapper)
        {
            this.mapper = mapper;
            this.animeService = animeService;
            this.episodeService = episodeService;
            this.LinksViewModel = new LinksViewModel();
        }

        private IMapper mapper;
        private AnimeService animeService;
        private EpisodeService episodeService;

        public int AnimeSeriesId { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public int EpisodeNumber { get; set; }

        public int EpisodeId { get; set; }

        [DefaultValue(false)]
        public bool IsLastEpisode { get; set; }

        public IEnumerable<AnimeLinkEnum> LinkSources { get; set; }

        public LinksViewModel LinksViewModel { get; set; }

        [BindProperty]
        public List<string> Links { get; set; }

        public async Task<IActionResult> OnGet(int id, int episode)
        {
            var anime = await this.animeService.FindAsync(id);
            if (anime == null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            //No value
            if (episode <= 0 || await this.episodeService.FindLastEpisodeNumberAsync(id) < episode)
            {
                await LoadData(id);
            }
            else
            {
                await LoadData(id, episode);
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPost(int animeSeriesId, int episodeNumber)
        {
            await this.LoadData(animeSeriesId);

            if (!this.Links.Any(p => p != null))
            {
                this.ModelState.AddModelError(nameof(this.Links), ErrorConstants.Invalid_Links_Selection);
                return this.Page();
            }

            var sourceLinks = await this.episodeService.GetAllSourceLinksAsync();

            for (int i = 0; i < this.Links.Count; i++)
            {
                var url = this.Links[i];

                //Link not added for current option
                if (url == null)
                {
                    continue;
                }

                // database id starts from 1 and c#'s lists from 0
                var source = sourceLinks.FirstOrDefault(p => p.Id == (i + 1));

                await this.episodeService.AddSourceLinkToEpisodeAsync(animeSeriesId, source.Id, url, episodeNumber);
            }

            return this.Redirect("/");
        }

        public async Task<IActionResult> OnGetRemoveLink(int sourceId,int animeSeriesId,int episodeNumber)
        {
            var episode = await this.episodeService.FindEpisodeAsync(animeSeriesId, episodeNumber);

            if ((await this.episodeService.GetAllLinksForAsync(episode.Id)).Count <= NumericConstants.Minimum_Links_For_Episode)
            {
                this.TempData.Add(ErrorConstants.Error_Key, ErrorConstants.Invalid_Link_Deletion);
            }
            else
            {
                await this.episodeService.RemoveEpisodeSourceLinkAsync(episode.Id, sourceId);
            }

            return this.Redirect($"/admin/addepisode/{animeSeriesId}?episode={episodeNumber}");
        }

        //In case of an error the properties need to be filled again
        private async Task LoadData(int id)
        {
            this.EpisodeNumber = await this.episodeService.FindLastEpisodeNumberAsync(id) + 1;

            var anime = await this.PartialLoad(id);

            this.IsLastEpisode = true;
        }

        private async Task LoadData(int id, int episode)
        {
            this.EpisodeNumber = episode;

            AnimeSeries anime = await PartialLoad(id);

            this.LinksViewModel.AnimeSeriesId = id;
            this.LinksViewModel.EpisodeNumber = this.EpisodeNumber;

            this.EpisodeId = (await this.episodeService.FindEpisodeAsync(id, this.EpisodeNumber)).Id;
        }

        private async Task<AnimeSeries> PartialLoad(int id)
        {
            var anime = await this.animeService.FindAsync(id);

            this.AnimeSeriesId = anime.Id;
            this.Title = anime.Title;
            this.ImageUrl = anime.ImageUrl;

            this.LinksViewModel.LinksAlreadyAdded = await this.episodeService.GetAllEpisodeLinksAsync(id, this.EpisodeNumber);
            this.LinksViewModel.Links = await this.episodeService.GetAllSourceLinksAsync();
            return anime;
        }
    }
}