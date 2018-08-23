using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAnimeWorld.Common.Animes.ViewModels;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Models;
using MyAnimeWorld.Services;

namespace MyAnimeWorld.App.Areas.Animes.Pages
{
    public class WatchModel : PageModel
    {
        public WatchModel(EpisodeService episodeService, AnimeService animeService, CategoryService categoryService, UserService profileService, UserManager<User> userManager)
        {
            this.EpisodeService = episodeService;
            this.AnimeService = animeService;
            this.CategoryService = categoryService;
            this.ProfileService = profileService;
            this.UserManager = userManager;
            this.WatchEpisodeViewModel = new WatchEpisodeViewModel();
        }

        private UserManager<User> UserManager { get; set; }

        private EpisodeService EpisodeService { get; set; }

        private AnimeService AnimeService { get; set; }

        private CategoryService CategoryService { get; set; }

        private UserService ProfileService { get; set; }

        public int AnimeSeriesId { get; set; }

        public int EpisodeId { get; set; }

        public int Rating { get; set; }

        public string Title { get; set; }

        public int EpisodeNumber { get; set; }

        public string SourceName { get; set; }

        public WatchEpisodeViewModel WatchEpisodeViewModel { get; set; }

        [DefaultValue(false)]
        public bool IsFavourite { get; set; }

        public IDictionary<int, int> Episodes { get; set; }

        public IDictionary<int, string> Categories { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        [BindProperty]
        public string Source { get; set; }

        [BindProperty]
        [Required(ErrorMessage = ErrorConstants.Invalid_Comment)]
        [MinLength(NumericConstants.Minimum_Comment_Length, ErrorMessage = ErrorConstants.Invalid_Comment)]
        public string CommentContent { get; set; }

        public async Task<IActionResult> OnGet(int id, [FromQuery]string source)
        {
            await LoadData(id, source);

            return this.Page();
        }

        public async Task<IActionResult> OnPost(int id, [FromQuery]string source)
        {
            await LoadData(id, source);

            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                this.TempData.Add(ErrorConstants.Error_Key, ErrorConstants.Uanuthorized_Comment_Attempt);
                return this.Redirect("/identity/account/login");
            }

            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            await this.EpisodeService.CreateComment(this.CommentContent, id, userId);

            this.Comments = await this.EpisodeService.LoadComments(id);

            var link = await this.LoadLink(id, source);

            //To delete comment content
            return this.Redirect($"/animes/watch/{id}?source={link.Source.Name}");
        }

        public async Task<IActionResult> OnPostFavouritesAsync(int id)
        {
            await this.LoadData(id, this.Source);
            var episode = await this.EpisodeService.FindAsync(id);

            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await this.ProfileService.GetUserByIdAsync(userId);

            if (user == null)
            {
                this.ModelState.AddModelError(ErrorConstants.Error_Key, ErrorConstants.Uanuthorized_Favourite_Add_Attempt);
                return this.Redirect("/identity/account/login");
            }

            if (!await this.ProfileService.ContainsFavouriteAnime(episode.AnimeSeriesId, userId))
            {
                await this.ProfileService.AddToFavourite(episode.AnimeSeriesId, user);
            }
            else
            {
                await this.ProfileService.RemoveFromFavourites(episode.AnimeSeriesId, user);
            }

            var link = await this.LoadLink(id, this.Source);

            return this.Redirect($"/animes/watch/{id}?source={link.Source.Name}");
        }

        public async Task<IActionResult> OnPostRatingsAsync(int id, int rating)
        {
            await this.LoadData(id, this.Source);
            var episode = await this.EpisodeService.FindAsync(id);

            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await this.ProfileService.GetUserByIdAsync(userId);

            if (user == null)
            {
                this.ModelState.AddModelError(ErrorConstants.Error_Key, ErrorConstants.Uanuthorized_Favourite_Add_Attempt);
                return this.Redirect("/identity/account/login");
            }

            await this.ProfileService.AddToRated(episode.AnimeSeriesId, user, rating);

            var link = await this.LoadLink(id, this.Source);

            return this.Redirect($"/animes/watch/{id}?source={link.Source.Name}");
        }

        public async Task<IActionResult> OnPostDeleteComment(int id,int commentId)
        {
            await this.ProfileService.DeleteCommentAsync(commentId);

            return this.Redirect($"/animes/watch/{id}");
        }

        private async Task LoadData(int id, [FromQuery]string source)
        {
            var episode = await this.EpisodeService.FindAsync(id);
            AnimeLink link = await LoadLink(id, source);

            await LoadProperties(episode, link);

            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await this.ProfileService.GetUserByIdAsync(userId);

            if (user != null)
            {
                var ratedAnime = await this.ProfileService.GetUserRatingsAsync(episode.AnimeSeriesId, userId);
                if (ratedAnime == null)
                {
                    this.IsFavourite = false;
                    this.Rating = 0;
                }
                else
                {
                    this.IsFavourite = ratedAnime.IsFavourite;
                    this.Rating = ratedAnime.Rating;
                }
            }

            this.ViewData["EpisodeId"] = id;
        }

        private async Task LoadProperties(AnimeEpisode episode, AnimeLink link)
        {
            this.EpisodeId = episode.Id;
            this.Title = await this.AnimeService.GetTitleAsync(episode.AnimeSeriesId);
            this.AnimeSeriesId = episode.AnimeSeriesId;
            this.Categories = await this.CategoryService.GetCategoriesIdNamePair(episode.AnimeSeriesId);
            this.EpisodeNumber = episode.EpisodeNumber;
            this.Episodes = await this.EpisodeService.GetAllEpisodesIdsAsync(episode.AnimeSeriesId);
            this.WatchEpisodeViewModel.Links = await this.EpisodeService.GetAnimeLinksForAsync(episode.Id, link.Source.Id);
            this.Comments = await this.EpisodeService.LoadComments(episode.Id);
            this.SourceName = link.Source.Name;
            this.WatchEpisodeViewModel.NextEpisodeId = await this.EpisodeService.GetNextEpisodeId(this.EpisodeNumber, this.AnimeSeriesId);
            this.WatchEpisodeViewModel.PreviousEpisodeId = await this.EpisodeService.GetPreviousEpisodeId(this.EpisodeNumber, this.AnimeSeriesId);
        }

        private async Task<AnimeLink> LoadLink(int id, string source)
        {
            var link = await this.EpisodeService.GetLinkByNameAsync(source);

            if (link == null)
            {
                //if user tries to modify url
                var workingLink = (await this.EpisodeService.GetAllLinksForAsync(id)).First();
                link = workingLink;
            }

            return link;
        }
    }
}