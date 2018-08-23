using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAnimeWorld.Common.Admin.BindingModels;
using MyAnimeWorld.Common.Main.ViewModels;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Data;
using MyAnimeWorld.Models;
using MyAnimeWorld.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAnimeWorld.Tests
{
    [TestClass]
    public class AnimeServiceTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Context = InitializedObjects.GetContext();
            AnimeService = InitializedObjects.GetAnimeService();
        }

        public AnimeWorldContext Context { get; set; }

        public AnimeService AnimeService { get; set; }

        [TestMethod]
        public async Task AddAnimeAsync_WithValidObject()
        {
            var animeSeries = this.Context.AnimeSeries.ToList();

            var animeBindingModel = new AddAnimeBindingModel()
            {
                CategoriesIds = new List<int>() { 1, 2, 3 },
                Description = "sjdasdja",
                Title = "test title",
                ImageUrl = DbConstants.Default_Avatar_Url
            };

            await this.AnimeService.AddAnimeAsync(animeBindingModel);

            var anime = this.Context.AnimeSeries.LastOrDefault();

            Assert.IsNotNull(anime);
            Assert.AreEqual(animeBindingModel.Title, anime.Title);
        }

        [TestMethod]
        public async Task AddAnimeAsync_WithInvalidObject()
        {
            var animeSeries = this.Context.AnimeSeries.ToList();

            AddAnimeBindingModel animeBindingModel = null;

            await Assert.ThrowsExceptionAsync<NullReferenceException>(() => this.AnimeService.AddAnimeAsync(animeBindingModel));
        }

        [TestMethod]
        public void FindAsync_WithValidObject()
        {
            var animeSeries = this.Context.AnimeSeries.ToList();

            var anime = this.Context.AnimeSeries.LastOrDefault();

            Assert.IsNotNull(this.AnimeService.FindAsync(anime?.Id ?? 0));
        }

        [TestMethod]
        public async Task FindAsync_WithInvalidObject()
        {
            var animeSeries = this.Context.AnimeSeries.ToList();

            var anime = this.Context.AnimeSeries.LastOrDefault();

            var test = await this.AnimeService.FindAsync((anime?.Id ?? 0) + 1);
            var test2 = await this.AnimeService.FindAsync(0);
            var test3 = await this.AnimeService.FindAsync(-4);

            Assert.IsNull(test);
            Assert.IsNull(test2);
            Assert.IsNull(test3);
        }

        [TestMethod]
        public async Task GetAnimeByTitleAsync_WithValidObject()
        {
            var animeSeries = this.Context.AnimeSeries.ToList();

            var anime = await this.AnimeService.GetAnimeByTitleAsync(animeSeries.FirstOrDefault()?.Title);

            Assert.AreEqual(animeSeries.FirstOrDefault(),anime);
        }

        [TestMethod]
        public async Task GetAnimeByTitleAsync_WithInvalidObject()
        {
            var anime = await this.AnimeService.GetAnimeByTitleAsync("Title that doesn't exist");
            var anime2 = await this.AnimeService.GetAnimeByTitleAsync("");
            var anime3 = await this.AnimeService.GetAnimeByTitleAsync(null);

            Assert.IsNull(anime);
            Assert.IsNull(anime2);
            Assert.IsNull(anime3);
        }

        [TestMethod]
        public async Task CreateAsync_WithValidObject()
        {
            var testTitle = "fefckejfke";

            var test = new AnimeSeries()
            {
                DateCreatedAt = DateTime.UtcNow,
                Description = "wgehwajew",
                ImageUrl = DbConstants.Default_Avatar_Url,
                Title = testTitle,
            };

            await this.AnimeService.CreateAsync(test);

            var anime = this.Context.AnimeSeries.FirstOrDefault(p => p.Title == testTitle);

            Assert.IsNotNull(anime);
        }

        [TestMethod]
        public async Task CreateAsync_WithInvalidObject()
        {
            AnimeSeries test = null;

            await Assert.ThrowsExceptionAsync<NullReferenceException>(() => this.AnimeService.CreateAsync(test));
        }

        [TestMethod]
        public async Task TitleExistsAsync_WithValidObject()
        {
            var animeSeries = this.Context.AnimeSeries.ToList();

            Assert.IsTrue(await this.AnimeService.TitleExistsAsync(animeSeries.FirstOrDefault()?.Title));
            Assert.IsTrue(await this.AnimeService.TitleExistsAsync(animeSeries.LastOrDefault()?.Title));
        }

        [TestMethod]
        public async Task TitleExistsAsync_WithInvalidObject()
        {
            Assert.IsFalse(await this.AnimeService.TitleExistsAsync("sadsdasda"));
            Assert.IsFalse(await this.AnimeService.TitleExistsAsync(""));
            Assert.IsFalse(await this.AnimeService.TitleExistsAsync(null));
        }

        [TestMethod]
        public async Task SearchAnimesAsync_WithValidObject()
        {
            var animeSeries = this.Context.AnimeSeries.ToList();
            
            Assert.IsNotNull(await this.AnimeService.SearchAnimesAsync(animeSeries.FirstOrDefault()?.Title));
            Assert.IsNotNull(await this.AnimeService.SearchAnimesAsync(animeSeries.LastOrDefault()?.Title));
            Assert.IsNotNull(await this.AnimeService.SearchAnimesAsync(animeSeries.LastOrDefault()?.Title, new List<int>() { 2, 3 }));
            Assert.IsNotNull(await this.AnimeService.SearchAnimesAsync(animeSeries.FirstOrDefault()?.Title, new List<int>() { 1 }));
        }

        [TestMethod]
        public async Task SearchAnimesAsync_WithInvalidObject()
        {
            Assert.AreEqual(0, (await this.AnimeService.SearchAnimesAsync("kerhereurh"))?.Count() ?? 0);
            Assert.AreEqual(0, (await this.AnimeService.SearchAnimesAsync(null))?.Count() ?? 0);
            Assert.AreEqual(0, (await this.AnimeService.SearchAnimesAsync(null, null))?.Count() ?? 0);
            Assert.AreEqual(0, (await this.AnimeService.SearchAnimesAsync("wrhw", null))?.Count() ?? 0);
        }

        [TestMethod]
        public async Task GetAllAnimesCountAsync_WithValidObjects()
        {
            var cnt = await this.AnimeService.GetAllAnimesCountAsync();

            Assert.AreEqual(this.Context.Episodes.Count(), cnt);

            var testEpisode = new AnimeEpisode()
            {
                AnimeSeriesId = 1,
                DateCreatedAt = DateTime.UtcNow,
                EpisodeNumber = 1,
            };
            var testEpisode2 = new AnimeEpisode()
            {
                AnimeSeriesId = 2,
                DateCreatedAt = DateTime.UtcNow,
                EpisodeNumber = 1,
            };

            this.Context.Episodes.Add(testEpisode);
            this.Context.Episodes.Add(testEpisode2);
            this.Context.SaveChanges();

            cnt = await this.AnimeService.GetAllAnimesCountAsync();

            Assert.AreEqual(this.Context.Episodes.Count(), cnt);
        }

        [TestMethod]
        public async Task GetAllAnimesSeriesCountAsync_WithValidObjects()
        {
            var cnt = await this.AnimeService.GetAllAnimesSeriesCountAsync();

            Assert.AreEqual(this.Context.AnimeSeries.Count(), cnt);

            var test = new AnimeSeries()
            {
                DateCreatedAt = DateTime.UtcNow,
                Description = "wgehwajew",
                ImageUrl = DbConstants.Default_Avatar_Url,
                Title = "fghurhgerighre",
            };
            var test2 = new AnimeSeries()
            {
                DateCreatedAt = DateTime.UtcNow,
                Description = "wgehwajew",
                ImageUrl = DbConstants.Default_Avatar_Url,
                Title = "bnrtihrotohbnfgjnbtr",
            };

            this.Context.AnimeSeries.Add(test);
            this.Context.AnimeSeries.Add(test2);
            this.Context.SaveChanges();

            cnt = await this.AnimeService.GetAllAnimesCountAsync();

            Assert.AreEqual(this.Context.Episodes.Count(), cnt);
        }


    }
}
