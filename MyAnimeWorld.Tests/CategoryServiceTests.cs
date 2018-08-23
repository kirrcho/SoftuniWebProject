using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAnimeWorld.Data;
using MyAnimeWorld.Models;
using MyAnimeWorld.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeWorld.Tests
{
    [TestClass]
    public class CategoryServiceTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Context = InitializedObjects.GetContext();
            CategoryService = new CategoryService(Context, InitializedObjects.GetMapper());
        }

        public AnimeWorldContext Context { get; set; }

        public CategoryService CategoryService { get; set; }

        [TestMethod]
        public async Task CategoryExistsAsync_WithValidObject()
        {
            var categoryTitle = this.Context.Categories.FirstOrDefault().Name;
            var categoryTitle2 = this.Context.Categories.LastOrDefault().Name;

            Assert.IsTrue(await this.CategoryService.CategoryExistsAsync(categoryTitle));
            Assert.IsTrue(await this.CategoryService.CategoryExistsAsync(categoryTitle2));
        }

        [TestMethod]
        public async Task CategoryExistsAsync_WithInvalidObject()
        {
            Assert.IsFalse(await this.CategoryService.CategoryExistsAsync("pesho"));
            Assert.IsFalse(await this.CategoryService.CategoryExistsAsync(null));
            Assert.IsFalse(await this.CategoryService.CategoryExistsAsync(string.Empty));
        }

        [TestMethod]
        public async Task FindAsync_WithValidObject()
        {
            var category = await this.Context.Categories.FirstOrDefaultAsync();

            var result = await this.CategoryService.FindAsync(category.Id);

            Assert.AreEqual(category, result);
        }

        [TestMethod]
        public async Task FindAsync_WithInvalidObject()
        {
            var lastCategoryId = (await this.Context.Categories.LastOrDefaultAsync()).Id;

            var result = await this.CategoryService.FindAsync(0);
            var result2 = await this.CategoryService.FindAsync(-32438);
            var result3 = await this.CategoryService.FindAsync(lastCategoryId + 1);

            Assert.IsNull(result);
            Assert.IsNull(result2);
            Assert.IsNull(result3);
        }

        [TestMethod]
        public async Task GetCategoryByNameAsync_WithValidObject()
        {
            var category = this.Context.Categories.FirstOrDefault();

            var result = await this.CategoryService.GetCategoryByNameAsync(category.Name);

            Assert.AreEqual(category, result);
        }

        [TestMethod]
        public async Task GetCategoryByNameAsync_WithInvalidObject()
        {
            var lastCategoryId = (await this.Context.Categories.LastOrDefaultAsync()).Id;

            var result = await this.CategoryService.FindAsync(0);
            var result2 = await this.CategoryService.FindAsync(-32438);
            var result3 = await this.CategoryService.FindAsync(lastCategoryId + 1);

            Assert.IsNull(result);
            Assert.IsNull(result2);
            Assert.IsNull(result3);
        }

        [TestMethod]
        public async Task GetAllCategoriesIdsForAsync_WithValidObject()
        {
            var animeSeries = this.Context.AnimeSeries.ToList();

            var realSeries = animeSeries.FirstOrDefault();
            var realSeries2 = animeSeries.LastOrDefault();

            var result = await this.CategoryService.GetAllCategoriesIdsForAsync(realSeries?.Id ?? 0);
            var result2 = await this.CategoryService.GetAllCategoriesIdsForAsync(realSeries2?.Id ?? 0);

            var categories = realSeries?.Categories?.Select(p => p.CategoryId).ToList();
            var categories2 = realSeries2?.Categories?.Select(p => p.CategoryId).ToList();

            CollectionAssert.AreEqual(categories, (List<int>)result);
            CollectionAssert.AreEqual(categories2, (List<int>)result2);
        }

        [TestMethod]
        public async Task GetAllCategoriesIdsForAsync_WithInvalidObject()
        {
            var emptyCollection = new List<int>();

            CollectionAssert.AreEqual(emptyCollection,(List<int>)await this.CategoryService.GetAllCategoriesIdsForAsync(0));
            CollectionAssert.AreEqual(emptyCollection,(List<int>)await this.CategoryService.GetAllCategoriesIdsForAsync(int.MinValue));
        }

        [TestMethod]
        public async Task AddCategoryAsync_WithValidObject()
        {
            var testingName = "gndlgfskrr";

            var category = new Category()
            {
                Name = testingName
            };

            await this.CategoryService.AddCategoryAsync(category);

            var result = this.Context.Categories.FirstOrDefault(p => p.Name == testingName);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task AddCategoryAsync_WithInvalidObject()
        {
            Category category = null;

            await this.CategoryService.AddCategoryAsync(category);
        }

        [TestMethod]
        public async Task AddAnimeSeriesCategoryAsync_WithValidObject()
        {
            var animeId = this.Context.AnimeSeries.FirstOrDefault().Id;
            var categoryId = this.Context.Categories.FirstOrDefault().Id;
            var animeId2 = this.Context.AnimeSeries.LastOrDefault().Id;
            var categoryId2 = this.Context.Categories.LastOrDefault().Id;

            var animeSeriesCategories = new AnimeSeriesCategories()
            {
                AnimeId = animeId,
                CategoryId = categoryId,
            };
            var animeSeriesCategories2 = new AnimeSeriesCategories()
            {
                AnimeId = animeId2,
                CategoryId = categoryId2,
            };

            await this.CategoryService.AddAnimeSeriesCategoryAsync(animeSeriesCategories);
            await this.CategoryService.AddAnimeSeriesCategoryAsync(animeSeriesCategories2);

            var result = this.Context.AnimeSeriesCategories.FirstOrDefault(p => p.AnimeId == animeId && p.CategoryId == categoryId);
            var result2 = this.Context.AnimeSeriesCategories.FirstOrDefault(p => p.AnimeId == animeId2 && p.CategoryId == categoryId2);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public async Task AddAnimeSeriesCategoryAsync_WithInvalidObject()
        {
            AnimeSeriesCategories animeSeriesCategories = null;

            await this.CategoryService.AddAnimeSeriesCategoryAsync(animeSeriesCategories);
        }

        [TestMethod]
        public async Task AnimeSeriesCategoryExistsAsync_WithValidObject()
        {
            var animeId = int.MaxValue;
            int categoryId = int.MaxValue;

            var animeCategories = new AnimeSeriesCategories()
            {
                AnimeId = animeId,
                CategoryId = categoryId
            };

            await this.Context.AnimeSeriesCategories.AddAsync(animeCategories);
            await this.Context.SaveChangesAsync();

            var result = await this.CategoryService.AnimeSeriesCategoryExistsAsync(animeId, categoryId);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task AnimeSeriesCategoryExistsAsync_WithInvalidObject()
        {
            var result = await this.CategoryService.AnimeSeriesCategoryExistsAsync(int.MinValue, 1);
            var result2 = await this.CategoryService.AnimeSeriesCategoryExistsAsync(0, int.MaxValue);
            var result3 = await this.CategoryService.AnimeSeriesCategoryExistsAsync(0,0);

            Assert.IsFalse(result);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }

        [TestMethod]
        public async Task GetAllCategoriesAsync_WithValidObject()
        {
            var cnt = (await this.CategoryService.GetAllCategoriesAsync()).ToList();

            CollectionAssert.AreEqual(this.Context.Categories.ToList(), cnt);

            var animeId = this.Context.AnimeSeries.FirstOrDefault().Id;
            int categoryId = this.Context.Categories.LastOrDefault().Id;

            var animeCategories = new AnimeSeriesCategories()
            {
                AnimeId = animeId,
                CategoryId = categoryId
            };

            await this.Context.AnimeSeriesCategories.AddAsync(animeCategories);
            await this.Context.SaveChangesAsync();

            cnt = (await this.CategoryService.GetAllCategoriesAsync()).ToList();

            CollectionAssert.AreEqual(this.Context.Categories.ToList(), cnt);
        }

        [TestMethod]
        public async Task GetCategoriesIdNamePair_WithValidObject()
        {
            var animeSeriesId = this.Context.Categories.FirstOrDefault().Id;

            var cnt = this.Context.Categories.ToDictionary(p => p.Id, k => k.Name);
            var cnt2 = this.Context.Categories.Where(p => p.Animes.Any(k => k.AnimeId == animeSeriesId)).ToDictionary(p => p.Id, k => k.Name);

            var result = (await this.CategoryService.GetCategoriesIdNamePair()).ToDictionary(p => p.Key,v => v.Value);
            var result2 = (await this.CategoryService.GetCategoriesIdNamePair(animeSeriesId)).ToDictionary(p => p.Key, v => v.Value);

            CollectionAssert.AreEqual(cnt, result);
            CollectionAssert.AreEqual(cnt2, result2);

            var category = new Category()
            {
                Name = "A name no one ever heard of"
            };

            await this.Context.Categories.AddAsync(category);
            await this.Context.SaveChangesAsync();

            cnt = this.Context.Categories.ToDictionary(p => p.Id, k => k.Name);
            cnt2 = this.Context.Categories.Where(p => p.Animes.Any(k => k.AnimeId == animeSeriesId)).ToDictionary(p => p.Id, k => k.Name);

            result = (await this.CategoryService.GetCategoriesIdNamePair()).ToDictionary(p => p.Key, v => v.Value);
            result2 = (await this.CategoryService.GetCategoriesIdNamePair(animeSeriesId)).ToDictionary(p => p.Key, v => v.Value);

            CollectionAssert.AreEqual(cnt, result);
            CollectionAssert.AreEqual(cnt2, result2);
        }
    }
}
