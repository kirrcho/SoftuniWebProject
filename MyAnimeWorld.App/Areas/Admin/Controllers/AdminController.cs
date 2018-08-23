using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAnimeWorld.Data;
using MyAnimeWorld.Services;

namespace MyAnimeWorld.App.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public abstract class AdminController : Controller
    {
        public AnimeService AnimeService { get; set; }

        public IMapper Mapper { get; set; }

        public AdminController(AnimeService animeService,IMapper mapper)
        {
            this.AnimeService = animeService;
            this.Mapper = mapper;
        }
    }
}
