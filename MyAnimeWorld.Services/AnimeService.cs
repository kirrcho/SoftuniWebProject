using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAnimeWorld.Common.Admin.BindingModels;
using MyAnimeWorld.Common.Animes.ViewModels;
using MyAnimeWorld.Common.Main.ViewModels;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Data;
using MyAnimeWorld.Models;

namespace MyAnimeWorld.Services
{
    public class AnimeService : BaseService
    {
        private CategoryService categoryService;

        public AnimeService(AnimeWorldContext animeWorldContext, IMapper mapper, CategoryService categoryService) : base(animeWorldContext, mapper)
        {
            this.categoryService = categoryService;
        }

        public async Task AddAnimeAsync(AddAnimeBindingModel model)
        {
            var anime = this.Mapper.Map<AnimeSeries>(model);
            await this.CreateAsync(anime);

            foreach (var category in model.CategoriesIds)
            {
                await this.AnimeContext.AnimeSeriesCategories.AddAsync(new AnimeSeriesCategories()
                {
                    AnimeId = anime.Id,
                    Category = await this.categoryService.FindAsync(category)
                });
            }

            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task<AnimeSeries> FindAsync(int id) => await this.AnimeContext.AnimeSeries.FindAsync(id);

        public async Task<AnimeSeries> GetAnimeByTitleAsync(string title) => await this.AnimeContext.AnimeSeries.FirstOrDefaultAsync(p => p.Title == title);

        public async Task CreateAsync(AnimeSeries animeSeries)
        {
            animeSeries.DateCreatedAt = DateTime.UtcNow;

            await this.AnimeContext.AnimeSeries.AddAsync(animeSeries);
            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task<bool> TitleExistsAsync(string title)
        {
            var anime = await this.AnimeContext.AnimeSeries.FirstOrDefaultAsync(p => p.Title == title);
            return anime != null;
        }

        public async Task<string> GetTitleAsync(int animeSeriesId)
        {
            var anime = await this.FindAsync(animeSeriesId);

            return anime.Title;
        }

        public async Task<ICollection<AnimeViewModel>> GetAnimesForPage(int pageNumber,bool orderByPopularity = false)
        {
            var animesToSkip = NumericConstants.Number_Of_Animes_Per_Page * (pageNumber - 1);

            List<AnimeViewModel> animes;

            if (orderByPopularity == true)
            {
                animes = await this.AnimeContext.Episodes
                    //When reaching property with no rating: Sum => null, Count => 0 (division with null or when you divide by 0 returns error)
                    .OrderByDescending(p => decimal.Parse(p.AnimeSeries.UsersFavouriteAnime.Sum(k => k.Rating).ToString() ?? "0") 
                    / (p.AnimeSeries.UsersFavouriteAnime.Count() == 0 ? 1 : p.AnimeSeries.UsersFavouriteAnime.Count))
                    .Skip(animesToSkip)
                    .Take(NumericConstants.Number_Of_Animes_Per_Page)
                    .Include(p => p.AnimeSeries)
                    .Select(p => new AnimeViewModel()
                    {
                        EpisodeId = p.Id,
                        EpisodeNumber = p.EpisodeNumber,
                        ImageUrl = p.AnimeSeries.ImageUrl,
                        Title = p.AnimeSeries.Title
                    }).ToListAsync();
            }
            else
            {
                animes = await this.AnimeContext.Episodes
                    .OrderByDescending(p => p.DateCreatedAt)
                    .Skip(animesToSkip)
                    .Take(NumericConstants.Number_Of_Animes_Per_Page)
                    .Include(p => p.AnimeSeries)
                    .Select(p => new AnimeViewModel()
                    {
                        EpisodeId = p.Id,
                        EpisodeNumber = p.EpisodeNumber,
                        ImageUrl = p.AnimeSeries.ImageUrl,
                        Title = p.AnimeSeries.Title
                    }).ToListAsync();
            }

            return animes;
        }

        public async Task<List<PopularAnimeSeriesViewModel>> GetPopularAnimesSeriesForPage(int pageNumber, bool orderByPopularity = false)
        {
            var animesToSkip = NumericConstants.Number_Of_Animes_Per_Page * (pageNumber - 1);

            List<PopularAnimeSeriesViewModel> animes;

            if (orderByPopularity == true)
            {
                animes = await this.AnimeContext.AnimeSeries
                    .Include(p => p.UsersFavouriteAnime)
                    //When reaching property with no rating: Sum => null, Count => 0 (division with null or when you divide by 0 returns error)
                    .OrderByDescending(p => decimal.Parse(p.UsersFavouriteAnime.Sum(k => k.Rating).ToString() ?? "0") / (p.UsersFavouriteAnime.Count() == 0 ? 1 : p.UsersFavouriteAnime.Count))
                    .Skip(animesToSkip)
                    .Take(NumericConstants.Number_Of_Animes_Per_Page)
                    .Select(p => new PopularAnimeSeriesViewModel()
                    {
                        AnimeSeriesId = p.Id,
                        ImageUrl = p.ImageUrl,
                        Title = p.Title,
                        //When reaching property with no rating: Sum => null, Count => 0 (division with null or when you divide by 0 returns error)
                        Rating = decimal.Parse(p.UsersFavouriteAnime.Sum(k => k.Rating).ToString() ?? "0") / (p.UsersFavouriteAnime.Count() == 0 ? 1 : p.UsersFavouriteAnime.Count)
                    }).ToListAsync();
            }
            else
            {
                animes = await this.AnimeContext.AnimeSeries
                    .OrderByDescending(p => p.DateCreatedAt)
                    .Skip(animesToSkip)
                    .Take(NumericConstants.Number_Of_Animes_Per_Page)
                    .Select(p => new PopularAnimeSeriesViewModel()
                    {
                        AnimeSeriesId = p.Id,
                        ImageUrl = p.ImageUrl,
                        Title = p.Title,
                        //When reaching property with no rating: Sum => null, Count => 0 (division with null or when you divide by 0 returns error)
                        Rating = decimal.Parse(p.UsersFavouriteAnime.Sum(k => k.Rating).ToString() ?? "0") / (p.UsersFavouriteAnime.Count() == 0 ? 1 : p.UsersFavouriteAnime.Count)
                    }).ToListAsync();
            }

            return animes;
        }
        
        public async Task<ICollection<AnimeSeriesViewModel>> GetAnimesSeriesForPage(int pageNumber,string userId)
        {
            var animesToSkip = NumericConstants.Number_Of_Animes_Per_Page * (pageNumber - 1);

            if (!this.AnimeContext.UserRatedAnimes.Any(p => p.UserId == userId && p.IsFavourite == true))
            {
                return new List<AnimeSeriesViewModel>();
            }

            var animes = await this.AnimeContext.UserRatedAnimes
                .Where(p => p.UserId == userId && p.IsFavourite == true)
                .OrderByDescending(p => p.AnimeSeries.DateCreatedAt)
                .Skip(animesToSkip)
                .Take(NumericConstants.Number_Of_Animes_Per_Page)
                .Include(p => p.AnimeSeries)
                .Select(p => new AnimeSeriesViewModel()
                {
                    AnimeSeriesId = p.AnimeSeries.Id,
                    ImageUrl = p.AnimeSeries.ImageUrl,
                    Title = p.AnimeSeries.Title
                }).ToListAsync();

            return animes;
        }

        public async Task<ICollection<AnimeSeriesViewModel>> GetAnimesSeriesForPage(int pageNumber)
        {
            var animesToSkip = NumericConstants.Number_Of_Animes_Per_Page * (pageNumber - 1);

            var animes = await this.AnimeContext.AnimeSeries
                .OrderByDescending(p => p.DateCreatedAt)
                .Skip(animesToSkip)
                .Take(NumericConstants.Number_Of_Animes_Per_Page)
                .Select(p => new AnimeSeriesViewModel()
                {
                    AnimeSeriesId = p.Id,
                    ImageUrl = p.ImageUrl,
                    Title = p.Title
                }).ToListAsync();

            return animes;
        }

        public async Task<IEnumerable<AnimeSeriesViewModel>> SearchAnimesAsync(string searchTerm)
        {
            if (searchTerm == null)
            {
                return null;
            }

            var animes = await this.AnimeContext.AnimeSeries.Where(p => p.Title.Contains(searchTerm)).ToListAsync();

            var viewModel = Mapper.Map<IEnumerable<AnimeSeriesViewModel>>(animes);

            return viewModel;
        }

        public async Task<IEnumerable<AnimeSeriesViewModel>> SearchAnimesAsync(string searchTerm,ICollection<int> InCategories)
        {
            if (InCategories == null || InCategories.Count == 0)
            {
                return null;
            }
            if (searchTerm == null)
            {
                return null;
            }

            var animes = await this.AnimeContext.AnimeSeries
                .Include(p => p.Categories)
                .Where(p => p.Categories.Any(k => InCategories.Contains(k.CategoryId)))
                .Where(p => p.Title.Contains(searchTerm))
                .ToListAsync();

            var viewModel = Mapper.Map<IEnumerable<AnimeSeriesViewModel>>(animes);

            return viewModel;
        }

        public async Task<IEnumerable<AnimeSeriesViewModel>> GetAnimesForCategory(string categoryName)
        {
            var category = await this.categoryService.GetCategoryByNameAsync(categoryName);

            if (category == null)
            {
                return null;
            }

            var animes = await this.AnimeContext.AnimeSeriesCategories
                .Where(p => p.CategoryId == category.Id)
                .Include(p => p.AnimeSeries)
                .Select(p => p.AnimeSeries)
                //Order by rating
                .OrderByDescending(p => (decimal)p.UsersFavouriteAnime.Sum(k => k.Rating) / p.UsersFavouriteAnime.Count)
                .ToListAsync();

            var animesViewModel = Mapper.Map<IEnumerable<AnimeSeriesViewModel>>(animes);

            return animesViewModel;
        }

        public async Task<IEnumerable<AnimeSeriesViewModel>> GetAnimesForCategory(string categoryName, int numberToTake)
        {
            var animes = (await this.GetAnimesForCategory(categoryName)).Take(numberToTake);

            return animes;
        }

        public async Task<int> GetAllAnimesCountAsync()
        {
            int cnt = await this.AnimeContext.Episodes.CountAsync();

            return cnt;
        }

        public async Task<int> GetAllAnimesSeriesCountAsync()
        {
            int cnt = await this.AnimeContext.AnimeSeries.CountAsync();

            return cnt;
        }

        public async Task<int> GetAllAnimesSeriesCountAsync(int animeSeriesId)
        {
            int cnt = await this.AnimeContext.AnimeSeries.CountAsync(p => p.Id == animeSeriesId);

            return cnt;
        }

        public async Task<int> GetAllAnimesSeriesCountAsync(string userId)
        {
            int cnt = await this.AnimeContext.UserRatedAnimes
                .Where(p => p.UserId == userId && p.IsFavourite == true)
                .Select(p => p.AnimeSeries)
                .CountAsync();

            return cnt;
        }

        public async Task DeleteSeries(int animeSeriesId)
        {
            var series = await this.AnimeContext.AnimeSeries
                .Include(p => p.Categories)
                .Include(p => p.Episodes)
                .Include(p => p.UsersFavouriteAnime)
                .FirstOrDefaultAsync(p => p.Id == animeSeriesId);

            if (series == null)
            {
                return;
            }

            this.AnimeContext.UserRatedAnimes.RemoveRange(series.UsersFavouriteAnime);
            foreach (var episodeTemplate in series.Episodes)
            {
                var episode = await this.AnimeContext.Episodes
                    .Include(p => p.Links)
                    .Include(p => p.Comments)
                    .FirstOrDefaultAsync(p => p.Id == episodeTemplate.Id);

                this.AnimeContext.Comments.RemoveRange(episode.Comments);
                this.AnimeContext.AnimeLinks.RemoveRange(episode.Links);
            }
            this.AnimeContext.Episodes.RemoveRange(series.Episodes);
            this.AnimeContext.AnimeSeriesCategories.RemoveRange(series.Categories);
            this.AnimeContext.AnimeSeries.Remove(series);
            await this.AnimeContext.SaveChangesAsync();
        }

        public List<int> LoadPages(int page,int pagesToLoad)
        {
            var list = this.LoadPageNumbersAlgorithm(page, NumericConstants.Index_Pagination_Number_Of_Pages,pagesToLoad);

            return list;
        }

        private List<int> LoadPageNumbersAlgorithm(int activePage, int count,int pagesToLoad)
        {
            var list = new List<int>();
            list.Add(activePage);

            int pageOverhead = 0;
            for (int i = 1; i <= (count + 1) / 2 - 1; i++)
            {
                if (activePage - i <= 0 && activePage + i <= pagesToLoad)
                {
                    list.Add(activePage + i - pageOverhead);
                }
                else if(activePage - i > 0)
                {
                    pageOverhead++;
                    list.Add(activePage - i);
                }
            }

            var pageToGoUpFrom = list.OrderByDescending(p => p).FirstOrDefault();

            for (int i = 1; i <= count / 2; i++)
            {
                if (i + pageToGoUpFrom <= pagesToLoad)
                {
                    list.Add(pageToGoUpFrom + i);
                }
            }

            return list;
        }
    }
}
