using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAnimeWorld.Data;
using MyAnimeWorld.Services;

namespace MyAnimeWorld.App.Areas.Animes.Controllers
{
    [Area("Animes")]
    public abstract class BaseController : Controller
    {
        public AnimeService AnimeService { get; set; }

        public IMapper Mapper { get; set; }

        public EpisodeService EpisodeService { get; set; }

        public CategoryService CategoryService { get; set; }

        public BaseController(AnimeService animeService,CategoryService categoryService,EpisodeService episodeService,IMapper mapper)
        {
            this.Mapper = mapper;
            this.AnimeService = animeService;
            this.CategoryService = categoryService;
            this.EpisodeService = episodeService;
        }
    }
}
