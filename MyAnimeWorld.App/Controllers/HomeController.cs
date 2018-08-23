using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyAnimeWorld.App.Models;
using MyAnimeWorld.Common.Main.ViewModels;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Services;

namespace MyAnimeWorld.App.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(AnimeService animeService)
        {
            this.AnimeService = animeService;
        }

        public AnimeService AnimeService { get; set; }
        
        public async Task<IActionResult> Index(int? id)
        {
            PagedAnimesViewModel viewModel = new PagedAnimesViewModel();

            if (id.HasValue && id > 0)
            {
                await this.LoadViewModelProperties(viewModel,id.Value);
            }
            else
            {
                await this.LoadViewModelProperties(viewModel, 1);
            }

            return this.View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task LoadViewModelProperties(PagedAnimesViewModel viewModel, int page)
        {
            int pagesToLoad = (int)Math.Ceiling((double)await this.AnimeService.GetAllAnimesCountAsync() / NumericConstants.Number_Of_Animes_Per_Page);

            if (page > pagesToLoad)
            {
                page = pagesToLoad;
            }

            viewModel.Animes = await this.AnimeService.GetAnimesForPage(page);
            viewModel.Pagination.Pages = this.AnimeService.LoadPages(page, pagesToLoad);
            viewModel.Pagination.PageUrl = UrlConstants.Index_Pagination;
            viewModel.Pagination.CurrentPage = page;
        }
    }
}
