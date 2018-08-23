using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAnimeWorld.Common.Main.ViewModels;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Models;
using MyAnimeWorld.Services;

namespace MyAnimeWorld.App.Areas.Animes.Pages
{
    public class New_ReleasesModel : PageModel
    {
        private readonly AnimeService animeService;

        public New_ReleasesModel(AnimeService animeService)
        {
            this.animeService = animeService;
            this.ViewModel = new PagedAnimeSeriesViewModel();
        }

        public PagedAnimeSeriesViewModel ViewModel { get; set; }

        public async Task OnGet(int? id)
        {
            if (id.HasValue && id > 0)
            {
                await this.LoadViewModelProperties(this.ViewModel, id.Value);
            }
            else
            {
                await this.LoadViewModelProperties(this.ViewModel, 1);
            }
        }

        private async Task LoadViewModelProperties(PagedAnimeSeriesViewModel viewModel, int page)
        {
            int pagesToLoad = (int)Math.Ceiling((double)await this.animeService.GetAllAnimesSeriesCountAsync() / NumericConstants.Number_Of_Animes_Per_Page);

            if (page > pagesToLoad)
            {
                page = pagesToLoad;
            }
            
            viewModel.Animes = await this.animeService.GetAnimesSeriesForPage(page);
            viewModel.Pagination.Pages = this.animeService.LoadPages(page, pagesToLoad);
            viewModel.Pagination.PageUrl = UrlConstants.New_Releases_Pagination;
            viewModel.Pagination.CurrentPage = page;
        }
    }
}