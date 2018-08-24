using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAnimeWorld.Common.Main.ViewModels;
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
                await this.LoadUsersViewModelProperties(viewModel, id.Value);
            }
            else
            {
                await this.LoadUsersViewModelProperties(viewModel, 1);
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
                await this.LoadUsersViewModelProperties(viewModel, id.Value);
            }
            else
            {
                await this.LoadUsersViewModelProperties(viewModel, 1);
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

        [HttpGet]
        [Route("/users/complaints/{id?}")]
        public async Task<IActionResult> Complaints(int? id)
        {
            var viewModel = new PagedComplaintsViewModel();

            if (id.HasValue && id > 0)
            {
                await this.LoadComplaintsViewModelProperties(viewModel, id.Value);
            }
            else
            {
                await this.LoadComplaintsViewModelProperties(viewModel, 1);
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [Route("/admin/complaints/remove")]
        public async Task<IActionResult> RemoveComplaint(int id)
        {
            await this.UserService.RemoveComplaint(id);

            this.TempData[SuccessConstants.Successful_Action_Key] = SuccessConstants.Successful_Deletion_Of_Complaint;

            return this.Redirect(UrlConstants.Complaints_Pagination);
        }

        private async Task LoadUsersViewModelProperties(PagedUsersViewModel viewModel, int page)
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

        private async Task LoadComplaintsViewModelProperties(PagedComplaintsViewModel viewModel, int page)
        {
            int pagesToLoad = (int)Math.Ceiling((double)await this.UserService.GetAllComplaintsCountAsync() / NumericConstants.Number_Of_Complaints_Per_Page);

            if (page > pagesToLoad)
            {
                page = pagesToLoad;
            }

            viewModel.Complaints = this.UserService.GetComplaintsForPage(page);
            viewModel.Pagination.Pages = this.AnimeService.LoadPages(page, pagesToLoad);
            viewModel.Pagination.PageUrl = UrlConstants.Complaints_Pagination;
            viewModel.Pagination.CurrentPage = page;
        }
    }
}
