using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyAnimeWorld.Models;
using System;

namespace MyAnimeWorld.Data
{
    public class AnimeWorldContext : IdentityDbContext<User>
    {
        public DbSet<User> AnimeUsers { get; set; }

        public DbSet<AnimeSeries> AnimeSeries { get; set; }

        public DbSet<AnimeEpisode> Episodes { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<AnimeLink> AnimeLinks { get; set; }

        public DbSet<AnimeLinkEnum> AnimeSourceLinks { get; set; }

        public DbSet<UserRatedAnime> UserRatedAnimes { get; set; }

        public DbSet<AnimeSeriesCategories> AnimeSeriesCategories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Complaint> Complaints { get; set; }

        public AnimeWorldContext(DbContextOptions<AnimeWorldContext> options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserRatedAnime>().HasKey(p => new { p.AnimeId, p.UserId });
            builder.Entity<UserRatedAnime>().HasOne(p => p.AnimeSeries).WithMany(p => p.UsersFavouriteAnime).HasForeignKey(p => p.AnimeId);
            builder.Entity<UserRatedAnime>().HasOne(p => p.User).WithMany(p => p.UserRatedAnimes).HasForeignKey(p => p.UserId);

            builder.Entity<AnimeSeriesCategories>().HasKey(p => new { p.AnimeId, p.CategoryId });
            builder.Entity<AnimeSeriesCategories>().HasOne(p => p.Category).WithMany(p => p.Animes).HasForeignKey(p => p.CategoryId);
            builder.Entity<AnimeSeriesCategories>().HasOne(p => p.AnimeSeries).WithMany(p => p.Categories).HasForeignKey(p => p.AnimeId);

            builder.Entity<AnimeSeries>().HasIndex(p => p.Title);

            base.OnModelCreating(builder);
        }
    }
}
