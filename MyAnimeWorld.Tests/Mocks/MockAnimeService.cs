using AutoMapper;
using MyAnimeWorld.Data;
using MyAnimeWorld.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Tests.Mocks
{
    public static class MockAnimeService
    {
        public static AnimeService GetAnimeService(AnimeWorldContext context,IMapper mapper)
        {
            var categoryService = new CategoryService(context, mapper);

            return new AnimeService(context, mapper, categoryService);
        }
    }
}
