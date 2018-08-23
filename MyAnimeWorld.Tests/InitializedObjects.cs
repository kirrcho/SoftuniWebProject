using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Data;
using MyAnimeWorld.Models;
using MyAnimeWorld.Services;
using MyAnimeWorld.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeWorld.Tests
{
    public static class InitializedObjects
    {
        public static AnimeService GetAnimeService() => AnimeService;

        public static AnimeWorldContext GetContext()
        {
            InitializeData(Context);
            return Context;
        }

        public static IMapper GetMapper() => Mapper;

        static InitializedObjects()
        {
            Context = MockContext.GetContext();
            Mapper = MockMapper.GetMapper();
            AnimeService = MockAnimeService.GetAnimeService(Context, Mapper);
        }

        private static AnimeWorldContext Context { get; set; }

        private static IMapper Mapper { get; set; }

        private static AnimeService AnimeService { get; set; }

        private static void InitializeData(AnimeWorldContext context)
        {
            var series = new AnimeSeries()
            {
                Description = "Testing Description",
                ImageUrl = DbConstants.Default_Avatar_Url,
                Title = "New test"
            };
            var series2 = new AnimeSeries()
            {
                Description = "Testing Description",
                ImageUrl = DbConstants.Default_Avatar_Url,
                Title = "New test2"
            };
            var series3 = new AnimeSeries()
            {
                Description = "Testing Description",
                ImageUrl = DbConstants.Default_Avatar_Url,
                Title = "New test3"
            };

            context.AnimeSeries.Add(series);
            context.AnimeSeries.Add(series2);
            context.AnimeSeries.Add(series3);

            var category = new Category()
            {
                Name = "first"
            };
            var category2 = new Category()
            {
                Name = "second"
            };
            var category3 = new Category()
            {
                Name = "third"
            };

            context.Categories.Add(category);
            context.Categories.Add(category2);
            context.Categories.Add(category3);
            
            var episode = new AnimeEpisode()
            {
                AnimeSeriesId = 1,
                DateCreatedAt = DateTime.UtcNow,
                EpisodeNumber = 1
            };

            context.Episodes.Add(episode);

            var animeLink = new AnimeLink()
            {
                AnimeId = 1,
                EpisodeId = 1,
                Source = new AnimeLinkEnum()
                {
                    Name = "first"
                },
                SourceUrl = "https://www.youtube.com/embed/aBn7bjy9c4U"
            };
            var animeLink2 = new AnimeLink()
            {
                AnimeId = 1,
                EpisodeId = 1,
                Source = new AnimeLinkEnum()
                {
                    Name = "second"
                },
                SourceUrl = "https://www.youtube.com/embed/aBn7bjy9c4U"
            };
            var animeLink3 = new AnimeLink()
            {
                AnimeId = 1,
                EpisodeId = 1,
                Source = new AnimeLinkEnum()
                {
                    Name = "third"
                },
                SourceUrl = "https://www.youtube.com/embed/aBn7bjy9c4U"
            };

            context.AnimeLinks.Add(animeLink);
            context.AnimeLinks.Add(animeLink2);
            context.AnimeLinks.Add(animeLink3);
            context.SaveChanges();
        }
    }
}
