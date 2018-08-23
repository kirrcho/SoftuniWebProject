using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAnimeWorld.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAnimeWorld.App.Areas.Categories.Controllers
{
    [Area("Categories")]
    public class CategoryController : Controller
    {
        public CategoryController(AnimeService animeService)
        {
            this.AnimeService = animeService;
        }

        public AnimeService AnimeService { get; set; }

        [HttpGet]
        [Route("/Categories/{categoryName}")]
        public async Task<IActionResult> Details(string categoryName)
        {
            var animes = await this.AnimeService.GetAnimesForCategory(categoryName);

            if (animes == null)
            {
                return NotFound();
            }

            return this.View(animes);
        }
    }
}
