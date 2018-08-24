using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Data;
using MyAnimeWorld.Models;
using MyAnimeWorld.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeWorld.Seed
{
    public static class DatabaseSeed
    {
        public static ResourceManager resourceManager = new ResourceManager("MyAnimeWorld.Seed.DescriptionData", Assembly.GetExecutingAssembly());

        public static IdentityRole[] IdentityRoles = new IdentityRole[]
        {
            new IdentityRole("Admin"),
            new IdentityRole("User"),
        };

        //ADMIN      Username = kireto, Password = kireto
        //Warning! All users added in here will have a password "kireto" unless you change the current seed method
        public static User[] Users = new User[]
        {
            new User()
            {
                Email = "kirrcho6@gmail.com",
                EmailConfirmed = true,
                UserName = "kireto",
                AvatarUrl = DbConstants.Default_Avatar_Url,
                 DateCreatedAt = DateTime.UtcNow,
                  
            }
        };

        public static Category[] Categories = new Category[]
        {
            new Category() { Name = "Action" },
            new Category() { Name = "Adventure" },
            new Category() { Name = "Comedy" },
            new Category() { Name = "Drama" },
            new Category() { Name = "Slice of Life" },
            new Category() { Name = "Fantasy" },
            new Category() { Name = "Magic" },
            new Category() { Name = "Supernatural" },
            new Category() { Name = "Horror" },
            new Category() { Name = "Mystery" },
            new Category() { Name = "Psychological" },
            new Category() { Name = "Romance" },
            new Category() { Name = "Sci-Fi" },
            new Category() { Name = "Ecchi" },
            new Category() { Name = "Game" },
            new Category() { Name = "Harem" },
            new Category() { Name = "Kids" },
            new Category() { Name = "Historical" },
            new Category() { Name = "Military" },
            new Category() { Name = "Mecha" },
            new Category() { Name = "School" },
            new Category() { Name = "Sports" },
            new Category() { Name = "Music" },
            new Category() { Name = "Shounen" },
        };

        public static AnimeLinkEnum[] SourceLinks = new AnimeLinkEnum[]
        {
            new AnimeLinkEnum() { Name = "Openload" },
            new AnimeLinkEnum() { Name = "Mp4Upload" },
            new AnimeLinkEnum() { Name = "VidStreaming" },
            new AnimeLinkEnum() { Name = "StreamAndGo" },
        };

        public static AnimeSeries[] Animes = new AnimeSeries[]
        {
            new AnimeSeries()
            {
                Description = resourceManager.GetString("Sword_Art_Online"),
                ImageUrl = "https://myanimelist.cdn-dena.com/images/anime/11/39717.jpg",
                Title = "Sword Art Online"
            },
            new AnimeSeries()
            {
                Description = resourceManager.GetString("Kimi_no_Na_wa"),
                ImageUrl = "https://myanimelist.cdn-dena.com/images/anime/5/87048.jpg",
                Title = "Kimi no Na wa"
            },
            new AnimeSeries()
            {
                Description = resourceManager.GetString("Shigatsu_wa_Kimi_no_Uso"),
                ImageUrl = "https://myanimelist.cdn-dena.com/images/anime/3/67177.jpg",
                Title = "Shigatsu wa Kimi no Uso"
            },
            new AnimeSeries()
            {
                Description = resourceManager.GetString("Death_Note"),
                ImageUrl = "https://myanimelist.cdn-dena.com/images/anime/9/9453.jpg",
                Title = "Death Note"
            }
        };

        public static AnimeSeriesCategories[] AnimeSeriesCategories = new AnimeSeriesCategories[]
        {
            new AnimeSeriesCategories()
            {
                AnimeId = 1,
                CategoryId = 1
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 1,
                CategoryId = 2
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 1,
                CategoryId = 6
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 1,
                CategoryId = 12
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 1,
                CategoryId = 15
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 2,
                CategoryId = 4
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 2,
                CategoryId = 8
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 2,
                CategoryId = 12
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 2,
                CategoryId = 21
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 3,
                CategoryId = 4
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 3,
                CategoryId = 12
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 3,
                CategoryId = 21
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 3,
                CategoryId = 23
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 3,
                CategoryId = 24
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 4,
                CategoryId = 10
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 4,
                CategoryId = 24
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 4,
                CategoryId = 8
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 4,
                CategoryId = 11
            },
            new AnimeSeriesCategories()
            {
                AnimeId = 4,
                CategoryId = 19
            },
        };

        public static AnimeEpisode[] AnimeEpisodes = new AnimeEpisode[]
        {
            new AnimeEpisode()
            {
                AnimeSeriesId = 1,
                EpisodeNumber = 1,
                Links = new List<AnimeLink>()
                {
                    new AnimeLink()
                    {
                        AnimeId = 1,
                        EpisodeId = 1,
                        SourceUrl = "https://www.mp4upload.com/embed-waqeqli33zhd.html",
                        SourceId = 2,
                    },
                    new AnimeLink()
                    {
                        AnimeId = 1,
                        EpisodeId = 1,
                        SourceUrl = "//vidstreaming.io/streaming.php?id=MzkzNDQ=&title=Sword+Art+Online+Episode+1",
                        SourceId = 3,
                    },
                }
            },
            new AnimeEpisode()
            {
                AnimeSeriesId = 1,
                EpisodeNumber = 2,
                Links = new List<AnimeLink>()
                {
                    new AnimeLink()
                    {
                        AnimeId = 1,
                        EpisodeId = 2,
                        SourceUrl = "https://www.mp4upload.com/embed-69h9v2l85cbx.html",
                        SourceId = 2,
                    },
                    new AnimeLink()
                    {
                        AnimeId = 1,
                        EpisodeId = 2,
                        SourceUrl = "//vidstreaming.io/streaming.php?id=MzkzNDU=&title=Sword+Art+Online+Episode+2",
                        SourceId = 3,
                    },
                }
            },
            new AnimeEpisode()
            {
                AnimeSeriesId = 1,
                EpisodeNumber = 3,
                Links = new List<AnimeLink>()
                {
                    new AnimeLink()
                    {
                        AnimeId = 1,
                        EpisodeId = 3,
                        SourceUrl = "https://www.mp4upload.com/embed-ve02jbxd9239.html",
                        SourceId = 2,
                    },
                    new AnimeLink()
                    {
                        AnimeId = 1,
                        EpisodeId = 3,
                        SourceUrl = "//vidstreaming.io/streaming.php?id=MzkzNDc=&title=Sword+Art+Online+Episode+3",
                        SourceId = 3,
                    },
                }
            },
            new AnimeEpisode()
            {
                AnimeSeriesId = 1,
                EpisodeNumber = 4,
                Links = new List<AnimeLink>()
                {
                    new AnimeLink()
                    {
                        AnimeId = 1,
                        EpisodeId = 4,
                        SourceUrl = "https://www.mp4upload.com/embed-cievrmqpr6w0.html",
                        SourceId = 2,
                    },
                    new AnimeLink()
                    {
                        AnimeId = 1,
                        EpisodeId = 4,
                        SourceUrl = "//vidstreaming.io/streaming.php?id=MzkzNDk=&title=Sword+Art+Online+Episode+4",
                        SourceId = 3,
                    },
                }
            },
            new AnimeEpisode()
            {
                AnimeSeriesId = 2,
                EpisodeNumber = 1,
                Links = new List<AnimeLink>()
                {
                    new AnimeLink()
                    {
                        AnimeId = 2,
                        EpisodeId = 5,
                        SourceUrl = "https://www.mp4upload.com/embed-unehyb7s07m8.html",
                        SourceId = 2,
                    },
                    new AnimeLink()
                    {
                        AnimeId = 2,
                        EpisodeId = 5,
                        SourceUrl = "//vidstreaming.io/streaming.php?id=NzU4ODg=&title=Kimi+no+Na+wa.+Episode+1",
                        SourceId = 3,
                    },
                    new AnimeLink()
                    {
                        AnimeId = 2,
                        EpisodeId = 5,
                        SourceUrl = "https://streamango.com/embed/csnleftmfdcfrmol",
                        SourceId = 4,
                    },
                }
            },
            new AnimeEpisode()
            {
                AnimeSeriesId = 3,
                EpisodeNumber = 1,
                Links = new List<AnimeLink>()
                {
                    new AnimeLink()
                    {
                        AnimeId = 3,
                        EpisodeId = 6,
                        SourceUrl = "https://openload.co/embed/LByh6uILv9g",
                        SourceId = 1,
                    },
                    new AnimeLink()
                    {
                        AnimeId = 3,
                        EpisodeId = 6,
                        SourceUrl = "https://www.mp4upload.com/embed-1g51hy9ve1k2.html",
                        SourceId = 2,
                    },
                    new AnimeLink()
                    {
                        AnimeId = 3,
                        EpisodeId = 6,
                        SourceUrl = "//vidstreaming.io/streaming.php?id=NDc4Njk=&title=Shigatsu+wa+Kimi+no+Uso+Episode+1",
                        SourceId = 3,
                    },
                    new AnimeLink()
                    {
                        AnimeId = 3,
                        EpisodeId = 6,
                        SourceUrl = "https://streamango.com/embed/nmodelqebnmpqsrl",
                        SourceId = 4,
                    },
                }
            },
            new AnimeEpisode()
            {
                AnimeSeriesId = 4,
                EpisodeNumber = 1,
                Links = new List<AnimeLink>()
                {
                    new AnimeLink()
                    {
                        AnimeId = 4,
                        EpisodeId = 7,
                        SourceUrl = "https://openload.co/embed/bSvL7BP9Rks",
                        SourceId = 1,
                    },
                    new AnimeLink()
                    {
                        AnimeId = 4,
                        EpisodeId = 7,
                        SourceUrl = "https://www.mp4upload.com/embed-4rywowfixnv7.html",
                        SourceId = 2,
                    },
                    new AnimeLink()
                    {
                        AnimeId = 4,
                        EpisodeId = 7,
                        SourceUrl = "//vidstreaming.io/streaming.php?id=MTA2MjQ=&title=Death+Note+Episode+1",
                        SourceId = 3,
                    },
                    new AnimeLink()
                    {
                        AnimeId = 4,
                        EpisodeId = 7,
                        SourceUrl = "https://streamango.com/embed/ketccenqoltorbpm",
                        SourceId = 4,
                    },
                }
            },
        };

        public static void Seed(this IApplicationBuilder app)
        {
            var serviceFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var scoped = serviceFactory.CreateScope();

            using (scoped)
            {
                RoleManager<IdentityRole> roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                UserManager<User> userManager = scoped.ServiceProvider.GetRequiredService<UserManager<User>>();
                AnimeService animeService = scoped.ServiceProvider.GetRequiredService<AnimeService>();
                EpisodeService episodeService = scoped.ServiceProvider.GetRequiredService<EpisodeService>();
                CategoryService categoryService = scoped.ServiceProvider.GetRequiredService<CategoryService>();

                SeedRoles(roleManager).GetAwaiter().GetResult();
                SeedUsers(userManager).GetAwaiter().GetResult();
                SeedCategories(categoryService).GetAwaiter().GetResult();
                SeedAnimeSeries(animeService).GetAwaiter().GetResult();
                SeedAnimeSeriesCategories(categoryService).GetAwaiter().GetResult();
                SeedSourceLinks(episodeService).GetAwaiter().GetResult();
                SeedEpisodes(episodeService).GetAwaiter().GetResult();
            }
        }

        private static async Task SeedAnimeSeries(AnimeService animeService)
        {
            foreach (var anime in Animes)
            {
                if (!await animeService.TitleExistsAsync(anime.Title))
                {
                    await animeService.CreateAsync(anime);
                }
            }
        }

        private static async Task SeedEpisodes(EpisodeService episodeService)
        {
            foreach (var episode in AnimeEpisodes)
            {
                if (!await episodeService.AnimeEpisodeExistsAsync(episode.AnimeSeriesId, episode.EpisodeNumber))
                {
                    foreach (var srcLink in episode.Links)
                    {
                        await episodeService.AddSourceLinkToEpisodeAsync(episode.AnimeSeriesId, srcLink.SourceId, srcLink.SourceUrl, episode.EpisodeNumber);
                    }
                }
            }
        }

        private static async Task SeedAnimeSeriesCategories(CategoryService categoryService)
        {
            foreach (var animeCategory in AnimeSeriesCategories)
            {
                if (!await categoryService.AnimeSeriesCategoryExistsAsync(animeCategory.AnimeId, animeCategory.CategoryId))
                {
                    await categoryService.AddAnimeSeriesCategoryAsync(animeCategory);
                }
            }
        }

        private static async Task SeedSourceLinks(EpisodeService episodeService)
        {
            foreach (var link in SourceLinks)
            {
                if (!await episodeService.SourceLinkExistsAsync(link.Name))
                {
                    await episodeService.AddAnimeLinkEnumAsync(link);
                }
            }
        }

        private static async Task SeedCategories(CategoryService categoryService)
        {
            foreach (var category in Categories)
            {
                if (!await categoryService.CategoryExistsAsync(category.Name))
                {
                    await categoryService.AddCategoryAsync(category);
                }
            }
        }

        private static async Task SeedUsers(UserManager<User> userManager)
        {
            foreach (var user in Users)
            {
                var account = await userManager.FindByNameAsync(user.UserName);
                if (account == null)
                {
                    await userManager.CreateAsync(user, "kireto");
                    await userManager.AddToRolesAsync(user, new string[] { "Admin", "User" });
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in IdentityRoles)
            {
                var exists = await roleManager.RoleExistsAsync(role.Name);
                if (!exists)
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}
