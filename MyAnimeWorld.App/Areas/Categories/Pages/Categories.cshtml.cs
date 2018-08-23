using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAnimeWorld.Common.Categories.ViewModels;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Services;

namespace MyAnimeWorld.App.Areas.Categories.Pages
{
    public class CategoriesModel : PageModel
    {
        public CategoriesModel(AnimeService animeService, CategoryService categoryService)
        {
            this.AnimeService = animeService;
            this.CategoryService = categoryService;
            this.Categories = new List<CategoryViewModel>();
        }

        private AnimeService AnimeService { get; set; }

        private CategoryService CategoryService { get; set; }

        public List<CategoryViewModel> Categories { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var categories = await this.CategoryService.GetAllCategoriesAsync();

            foreach (var category in categories)
            {
                var animes = await this.AnimeService.GetAnimesForCategory(category.Name, NumericConstants.Default_Animes_Number_For_Category);

                this.Categories.Add(new CategoryViewModel()
                {
                    Name = category.Name,
                    Animes = animes
                });
            }

            return this.Page();
        }
    }
}