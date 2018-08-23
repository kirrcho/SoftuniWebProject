using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MyAnimeWorld.Common.Animes.ViewModels;
using MyAnimeWorld.Common.Main.ViewModels;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Data;
using MyAnimeWorld.Services;

namespace MyAnimeWorld.App.Areas.Animes.Controllers
{
    public class AnimeController : BaseController
    {
        public UserService ProfileService { get; set; }

        public AnimeController(AnimeService animeService,CategoryService categoryService,EpisodeService episodeService,UserService profileService,IMapper mapper) 
            : base(animeService,categoryService,episodeService,mapper)
        {
            this.ProfileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var anime = await this.AnimeService.FindAsync(id);

            var viewModel = this.Mapper.Map<AnimeDetailsViewModel>(anime);

            viewModel.Categories = (await this.CategoryService.GetCategoriesIdNamePair(id))
                .Select(p => p.Value);

            viewModel.Episodes = await this.EpisodeService.GetAllEpisodesIdsAsync(id);

            //If userId is null then rating and isfavourite don't exist and if he never rated the anime at all they still won't exist
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                viewModel.Rating = 0;
                viewModel.IsFavourite = false;
            }
            else
            {
                await this.LoadFavouriteBoolAndRatings(id, userId, viewModel);
            }

            return this.View(viewModel);
        }

        [HttpPost()]
        public async Task<IActionResult> Details(int id,int rating,string submitType)
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await this.ProfileService.GetUserByIdAsync(userId);

            if (user == null)
            {
                this.ModelState.AddModelError(ErrorConstants.Error_Key, ErrorConstants.Uanuthorized_Favourite_Add_Attempt);
                return this.Redirect("/identity/account/login");
            }

            //submitType is the value of all buttons in the form
            if (submitType == "Add to Favourites")
            {
                if (!await this.ProfileService.ContainsFavouriteAnime(id, userId))
                {
                    await this.ProfileService.AddToFavourite(id, user);
                }
            }
            else if (submitType == "Remove from Favourites")
            {
                await this.ProfileService.RemoveFromFavourites(id, user);
            }
            else if (submitType == "Submit Rating")
            {
                await this.ProfileService.AddToRated(id, user, rating);
            }

            var anime = await this.AnimeService.FindAsync(id);

            var viewModel = this.Mapper.Map<AnimeDetailsViewModel>(anime);

            viewModel.Categories = (await this.CategoryService.GetCategoriesIdNamePair(id))
                .Select(p => p.Value);

            viewModel.Episodes = await this.EpisodeService.GetAllEpisodesIdsAsync(id);

            await LoadFavouriteBoolAndRatings(id, userId, viewModel);

            return this.View(viewModel);
        }

        [HttpGet]
        [Route("/Animes/Popular/{id?}")]
        public async Task<IActionResult> Popular(int? id)
        {
            PagedPopularAnimeSeriesViewModel viewModel = new PagedPopularAnimeSeriesViewModel();
            
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
        
        private async Task LoadViewModelProperties(PagedPopularAnimeSeriesViewModel viewModel, int page)
        {
            int pagesToLoad = (int)Math.Ceiling((double)await this.AnimeService.GetAllAnimesSeriesCountAsync() / NumericConstants.Number_Of_Animes_Per_Page);

            if (page > pagesToLoad)
            {
                page = pagesToLoad;
            }
            
            viewModel.Animes = await this.AnimeService.GetPopularAnimesSeriesForPage(page,true);
            viewModel.Pagination.Pages = this.AnimeService.LoadPages(page, pagesToLoad);
            viewModel.Pagination.PageUrl = UrlConstants.Popular_Pagination;
            viewModel.Pagination.CurrentPage = page;
        }

        private async Task LoadFavouriteBoolAndRatings(int id, string userId, AnimeDetailsViewModel viewModel)
        {
            var ratedAnime = await this.ProfileService.GetUserRatingsAsync(id, userId);
            if (ratedAnime == null)
            {
                viewModel.IsFavourite = false;
                viewModel.Rating = 0;
            }
            else
            {
                viewModel.IsFavourite = ratedAnime.IsFavourite;
                viewModel.Rating = ratedAnime.Rating;
            }
        }
    }
}
