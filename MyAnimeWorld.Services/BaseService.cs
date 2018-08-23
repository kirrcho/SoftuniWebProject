using AutoMapper;
using MyAnimeWorld.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Services
{
    public abstract class BaseService
    {
        public BaseService(AnimeWorldContext animeWorldContext,IMapper mapper)
        {
            this.AnimeContext = animeWorldContext;
            this.Mapper = mapper;
        }

        protected IMapper Mapper { get; set; }

        protected AnimeWorldContext AnimeContext { get; set; }
    }
}
