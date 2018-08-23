using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAnimeWorld.Common.Main.ViewModels;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Models;
using MyAnimeWorld.Services;

namespace MyAnimeWorld.App.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "User")]
    public class FavouritesModel : PageModel
    {
        private readonly UserService profileService;
        private readonly AnimeService animeService;

        public PagedAnimeSeriesViewModel ViewModel { get; set; }

        public string Username { get; set; }

        public FavouritesModel(UserService profileService, AnimeService animeService)
        {
            this.profileService = profileService;
            this.animeService = animeService;
            this.ViewModel = new PagedAnimeSeriesViewModel();
        }

        public async Task<IActionResult> OnGet(string username, int? id)
        {
            User user;
            
            if (username == null || username == string.Empty)
            {
                string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                user = await this.profileService.GetUserByIdAsync(userId);
            }
            else
            {
                user = await this.profileService.GetUserByUsernameAsync(username);
            }
            
            if (id.HasValue && id > 0)
            {
                await this.LoadViewModelProperties(this.ViewModel, id.Value,user);
            }
            else
            {
                await this.LoadViewModelProperties(this.ViewModel, 1,user);
            }

            return this.Page();
        }

        private async Task LoadViewModelProperties(PagedAnimeSeriesViewModel viewModel, int page,User user)
        {
            int pagesToLoad = (int)Math.Ceiling((double)await this.animeService.GetAllAnimesSeriesCountAsync(user.Id) / NumericConstants.Number_Of_Animes_Per_Page);
            
            if (page > pagesToLoad)
            {
                page = pagesToLoad;
            }

            this.Username = user.UserName;
            viewModel.Animes = await this.animeService.GetAnimesSeriesForPage(page,user.Id);
            viewModel.Pagination.Pages = this.animeService.LoadPages(page, pagesToLoad);
            viewModel.Pagination.PageUrl = UrlConstants.Favourites_Pagination + $"{user.UserName}/";
            viewModel.Pagination.CurrentPage = page;
        }
    }
}