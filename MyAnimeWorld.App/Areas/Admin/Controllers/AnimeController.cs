using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAnimeWorld.Common.Admin.BindingModels;
using MyAnimeWorld.Common.Admin.ViewModels;
using MyAnimeWorld.Common.Main.ViewModels;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Services;

namespace MyAnimeWorld.App.Areas.Admin.Controllers
{
    [Authorize()]
    public class AnimeController : AdminController
    {
        public AnimeController(AnimeService animeService, EpisodeService episodeService, CategoryService categoryService, IMapper mapper) : base(animeService, mapper)
        {
            this.EpisodeService = episodeService;
            this.CategoryService = categoryService;
        }

        public EpisodeService EpisodeService { get; set; }

        public CategoryService CategoryService { get; set; }

        [HttpGet]
        [Route("/admin/addanime")]
        public async Task<IActionResult> AddAnime()
        {
            await LoadAddAnimeData();

            return this.View();
        }

        [HttpPost]
        [Route("/admin/addanime")]
        public async Task<IActionResult> AddAnime(AddAnimeBindingModel model)
        {
            await LoadAddAnimeData();

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            if (model.CategoriesIds == null || model.CategoriesIds.Count <= 0)
            {
                this.ModelState.AddModelError(nameof(model.CategoriesIds), ErrorConstants.Invalid_Category_Selection);
                return this.View(model);
            }
            if (await this.AnimeService.TitleExistsAsync(model.Title))
            {
                this.ModelState.AddModelError(nameof(model.Title), ErrorConstants.Invalid_Title_Already_Exists);
                return this.View(model);
            }

            await this.AnimeService.AddAnimeAsync(model);

            var anime = await this.AnimeService.GetAnimeByTitleAsync(model.Title);

            return Redirect($"/admin/addepisode/{anime.Id}");
        }

        [HttpGet]
        [Route("/admin/series/{id?}")]
        public async Task<IActionResult> Series(int? id)
        {
            PagedAnimeSeriesViewModel viewModel = new PagedAnimeSeriesViewModel();
            if (id.HasValue && id > 0)
            {
                await this.LoadViewModelProperties(viewModel, id.Value);
            }
            else
            {
                await this.LoadViewModelProperties(viewModel, 1);
            }

            return this.View(viewModel);
        }

        [HttpGet]
        [Route("/admin/deleteseries/{id}")]
        public async Task<IActionResult> DeleteSeries(int id)
        {
            var animeSeries = await this.AnimeService.FindAsync(id);
            var episodes = await this.EpisodeService.GetAllEpisodesCount(animeSeries.Id);

            if (animeSeries == null)
            {
                return NotFound();
            }

            var viewModel = new DeleteAnimeSeriesViewModel()
            {
                EpisodesDeleted = episodes,
                Title = animeSeries.Title
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [Route("/admin/deleteseries/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await this.AnimeService.DeleteSeries(id);

            this.TempData["message"] = "Anime series deleted successfully";

            return this.Redirect("/admin/deleteseries/success");
        }

        [HttpPost]
        [Route("/admin/deleteepisode/{id}")]
        public async Task<IActionResult> DeleteEpisode(int id)
        {
            await this.EpisodeService.DeleteEpisode(id);

            this.TempData["message"] = "Episode deleted successfully";

            return this.Redirect("/admin/deleteseries/success");
        }

        [HttpGet]
        [Route("/admin/deleteseries/success")]
        public IActionResult Success()
        {
            var message = this.TempData["message"];

            if (message == null)
            {
                return NotFound();
            }

            return this.View(message);
        }
        private async Task LoadViewModelProperties(PagedAnimeSeriesViewModel viewModel, int page)
        {
            int pagesToLoad = (int)Math.Ceiling((double)await this.AnimeService.GetAllAnimesSeriesCountAsync() / NumericConstants.Number_Of_Animes_Per_Page);

            if (page > pagesToLoad)
            {
                page = pagesToLoad;
            }

            viewModel.Animes = await this.AnimeService.GetAnimesSeriesForPage(page);
            viewModel.Pagination.Pages = this.AnimeService.LoadPages(page, pagesToLoad);
            viewModel.Pagination.PageUrl = UrlConstants.Admin_AnimeSeries_Pagination;
            viewModel.Pagination.CurrentPage = page;
        }

        private async Task LoadAddAnimeData()
        {
            var categories = await this.CategoryService.GetCategoriesIdNamePair();

            this.ViewData["Categories"] = categories;
        }
    }
}
