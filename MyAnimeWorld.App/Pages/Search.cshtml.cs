using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAnimeWorld.Common.Main.ViewModels;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Services;

namespace MyAnimeWorld.App.Pages
{
    public class SearchModel : PageModel
    {
        public SearchModel(AnimeService animeService, CategoryService categoryService)
        {
            this.AnimeService = animeService;
            this.CategoryService = categoryService;
        }

        private AnimeService AnimeService { get; set; }

        private CategoryService CategoryService { get; set; }

        public IEnumerable<AnimeSeriesViewModel> SearchResults { get; set; }

        public string SearchTerm { get; set; }

        public IDictionary<int,string> Categories { get; set; }

        [BindProperty]
        public List<int> CategoriesIds { get; set; }

        public async Task<IActionResult> OnGet(string searchTerm)
        {
            if (searchTerm == null || searchTerm.Trim().Length < NumericConstants.Minimum_Search_Term_Length)
            {
                //this.ViewData.ModelState.AddModelError(ErrorConstants.Error_Key, ErrorConstants.Invalid_Search_Term);
                this.TempData[ErrorConstants.Error_Key] = ErrorConstants.Invalid_Search_Term;
                return this.RedirectToAction("Index", "Home");
            }

            this.SearchResults = await this.AnimeService.SearchAnimesAsync(searchTerm);

            this.Categories = await this.CategoryService.GetCategoriesIdNamePair();

            this.SearchTerm = searchTerm;

            this.ViewData["CheckedBoxes"] = this.Categories.Keys;

            return this.Page();
        }

        public async Task<IActionResult> OnPost(string searchTerm)
        {
            if (searchTerm == null || searchTerm.Trim().Length < NumericConstants.Minimum_Search_Term_Length)
            {
                this.ModelState.AddModelError(ErrorConstants.Error_Key, ErrorConstants.Invalid_Search_Term);
                return this.RedirectToAction("Index", "Home");
            }

            this.SearchResults = await this.AnimeService.SearchAnimesAsync(searchTerm,this.CategoriesIds);

            this.Categories = await this.CategoryService.GetCategoriesIdNamePair();

            this.ViewData["CheckedBoxes"] = this.CategoriesIds;

            this.SearchTerm = searchTerm;

            return this.Page();
        }
    }
}