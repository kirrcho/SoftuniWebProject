using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAnimeWorld.Common.Main.ViewModels;
using MyAnimeWorld.Data;
using MyAnimeWorld.Models;

namespace MyAnimeWorld.Services
{
    public class CategoryService : BaseService
    {
        public CategoryService(AnimeWorldContext animeWorldContext, IMapper mapper) : base(animeWorldContext, mapper) { }

        public async Task<bool> CategoryExistsAsync(string categoryName)
        {
            var category = await this.AnimeContext.Categories.FirstOrDefaultAsync(p => p.Name == categoryName);
            return category != null;
        }

        public async Task<Category> FindAsync(int categoryId)
        {
            var category = await this.AnimeContext.Categories.FindAsync(categoryId);
            return category;
        }

        public async Task<Category> GetCategoryByNameAsync(string categoryName)
        {
            categoryName = categoryName.ToLower();

            var category = await this.AnimeContext.Categories.FirstOrDefaultAsync(p => p.Name.ToLower() == categoryName);
            return category;
        }

        public async Task<IEnumerable<int>> GetAllCategoriesIdsForAsync(int animeSeriesId)
        {
            var categoriesIds = await this.AnimeContext.AnimeSeriesCategories.Where(p => p.AnimeId == animeSeriesId)
                .Select(p => p.Category.Id).ToListAsync();
            return categoriesIds;
        }

        public async Task AddCategoryAsync(Category category)
        {
            if (category == null)
            {
                return;
            }

            await this.AnimeContext.Categories.AddAsync(category);
            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task AddAnimeSeriesCategoryAsync(AnimeSeriesCategories animeSeriesCategory)
        {
            if (animeSeriesCategory == null)
            {
                return;
            }

            await this.AnimeContext.AnimeSeriesCategories.AddAsync(animeSeriesCategory);
            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task<bool> AnimeSeriesCategoryExistsAsync(int animeSeriesId, int categoryId)
        {
            var animeCategory = await this.AnimeContext.AnimeSeriesCategories.FirstOrDefaultAsync(p => p.AnimeId == animeSeriesId && p.CategoryId == categoryId);

            return animeCategory != null;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = await this.AnimeContext.Categories.ToListAsync();
            return categories;
        }

        public async Task<IDictionary<int, string>> GetCategoriesIdNamePair()
        {
            var categories = await this.GetAllCategoriesAsync();
            Dictionary<int, string> dictionary = this.LoadCategoriesIdNamePair(categories);

            return dictionary;
        }

        public async Task<IDictionary<int, string>> GetCategoriesIdNamePair(int animeSeriesId)
        {
            var categories = await this.AnimeContext.AnimeSeriesCategories
                .Where(p => p.AnimeId == animeSeriesId)
                .Include(p => p.Category)
                .Select(p => p.Category)
                .ToListAsync();

            Dictionary<int, string> dictionary = this.LoadCategoriesIdNamePair(categories);

            return dictionary;
        }

        private Dictionary<int, string> LoadCategoriesIdNamePair(IEnumerable<Category> categories)
        {
            var dictionary = new Dictionary<int, string>();

            foreach (var category in categories)
            {
                dictionary.Add(category.Id, category.Name);
            }

            return dictionary;
        }
    }
}
