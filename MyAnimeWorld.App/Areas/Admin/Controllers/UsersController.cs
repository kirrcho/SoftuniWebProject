using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAnimeWorld.Common.Users.ViewModels;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Services;

namespace MyAnimeWorld.App.Areas.Admin.Controllers
{
    public class UsersController : AdminController
    {
        public UserService UserService { get; set; }

        public UsersController(AnimeService animeService,UserService userService, IMapper mapper) : base(animeService, mapper)
        {
            this.UserService = userService;
        }

        [HttpGet]
        [Route("/users/all/{id?}")]
        public async Task<IActionResult> All(int? id)
        {
            var viewModel = new PagedUsersViewModel();

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

        [HttpPost]
        [Route("/users/all/{id?}")]
        public async Task<IActionResult> All(int? id,string searchTerm)
        {
            var viewModel = new PagedUsersViewModel();

            if (id.HasValue && id > 0)
            {
                await this.LoadViewModelProperties(viewModel, id.Value);
            }
            else
            {
                await this.LoadViewModelProperties(viewModel, 1);
            }

            if (searchTerm == null || searchTerm.Length <= NumericConstants.Minimum_Search_Term_Length)
            {
                this.TempData.Add(ErrorConstants.Error_Key, ErrorConstants.Invalid_Search_Term);
            }
            else
            {
                viewModel.SearchTerm = searchTerm;
                viewModel.Users = viewModel.Users.Where(p => p.Username.Contains(searchTerm));
            }

            return this.View(viewModel);
        }

        private async Task LoadViewModelProperties(PagedUsersViewModel viewModel, int page)
        {
            int pagesToLoad = (int)Math.Ceiling((double)await this.UserService.GetAllUsersCountAsync() / NumericConstants.Number_Of_Users_Per_Page);

            if (page > pagesToLoad)
            {
                page = pagesToLoad;
            }

            viewModel.Users = await this.UserService.GetUsersForPage(page);
            viewModel.Pagination.Pages = this.AnimeService.LoadPages(page, pagesToLoad);
            viewModel.Pagination.PageUrl = UrlConstants.Users_Pagination;
            viewModel.Pagination.CurrentPage = page;
        }
    }
}
