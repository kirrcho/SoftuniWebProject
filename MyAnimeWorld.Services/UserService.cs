using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyAnimeWorld.Common.Main.BindingModels;
using MyAnimeWorld.Common.Main.ViewModels;
using MyAnimeWorld.Common.Users.ViewModels;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Data;
using MyAnimeWorld.Models;

namespace MyAnimeWorld.Services
{
    public class UserService : BaseService
    {
        public UserService(AnimeWorldContext animeWorldContext, IMapper mapper) : base(animeWorldContext, mapper) {  }

        public async Task ChangeAvatarAsync(string userId,string avatarUrl)
        {
            var user = await this.AnimeContext.Users.FirstOrDefaultAsync(p => p.Id == userId);

            user.AvatarUrl = avatarUrl;
            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await this.AnimeContext.Users.FirstOrDefaultAsync(p => p.UserName == username);

            return user;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var user = await this.AnimeContext.Users.FirstOrDefaultAsync(p => p.Id == id);

            return user;
        }

        public async Task SetAvatarAsync(User user,string avatarUrl)
        {
            user.AvatarUrl = avatarUrl;
            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task<bool> ContainsFavouriteAnime(int animeSeriesId,string userId)
        {
            var favourites = await this.AnimeContext.UserRatedAnimes
                .FirstOrDefaultAsync(p => p.AnimeId == animeSeriesId && p.UserId == userId && p.IsFavourite == true);

            return favourites != null ? true : false;
        }

        public async Task<UserRatedAnime> GetUserRatingsAsync(int animeSeriesId,string userId)
        {
            var ratedAnime = await this.AnimeContext.UserRatedAnimes
                .FirstOrDefaultAsync(p => p.AnimeId == animeSeriesId && p.UserId == userId);

            return ratedAnime;
        }

        public async Task AddToFavourite(int animeSeriesId, User user)
        {
            UserRatedAnime favourite = await this.GetUserRatingsAsync(animeSeriesId, user.Id);

            if (favourite == null)
            {
                favourite = new UserRatedAnime()
                {
                    AnimeId = animeSeriesId,
                    IsFavourite = true,
                    UserId = user.Id
                };

                await this.AnimeContext.UserRatedAnimes.AddAsync(favourite);
                user.UserRatedAnimes.Add(favourite);
            }
            else
            {
                favourite.IsFavourite = true;
            }

            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task AddToRated(int animeSeriesId, User user,int rating)
        {
            UserRatedAnime favourite = await this.AnimeContext.UserRatedAnimes.FirstOrDefaultAsync(p => p.AnimeId == animeSeriesId && p.UserId == user.Id);

            if (favourite != null)
            {
                favourite.Rating = rating;
            }
            else
            {
                favourite = new UserRatedAnime()
                {
                    AnimeId = animeSeriesId,
                    IsFavourite = false,
                    UserId = user.Id,
                    Rating = rating
                };

                await this.AnimeContext.UserRatedAnimes.AddAsync(favourite);
                user.UserRatedAnimes.Add(favourite);
            }

            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task RemoveFromFavourites(int animeSeriesId, User user)
        {
            var favourite = await this.AnimeContext.UserRatedAnimes
                .FirstOrDefaultAsync(p => p.AnimeId == animeSeriesId && p.IsFavourite == true && p.UserId == user.Id);

            if (favourite == null)
            {
                return;
            }

            favourite.IsFavourite = false;
            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserViewModel>> GetUsersForPage(int page)
        {
            var usersToSkip = NumericConstants.Number_Of_Users_Per_Page * (page - 1);

            if (!this.AnimeContext.Users.Any())
            {
                return new List<UserViewModel>();
            }

            var users = await this.AnimeContext.Users
                .OrderByDescending(p => p.DateCreatedAt)
                .Skip(usersToSkip)
                .Take(NumericConstants.Number_Of_Users_Per_Page)
                .Select(p => new UserViewModel
                {
                    Username = p.UserName,
                    Avatar = p.AvatarUrl
                }).ToListAsync();

            return users;
        }

        public IEnumerable<ComplaintViewModel> GetComplaintsForPage(int page)
        {
            var complaintsToSkip = NumericConstants.Number_Of_Complaints_Per_Page * (page - 1);

            if (!this.AnimeContext.Complaints.Any())
            {
                return new List<ComplaintViewModel>();
            }

            var complaints = this.AnimeContext.Complaints
                .OrderByDescending(p => p.DateCreatedAt)
                .Skip(complaintsToSkip)
                .Take(NumericConstants.Number_Of_Complaints_Per_Page);

            var model =  this.Mapper.Map<IEnumerable<ComplaintViewModel>>(complaints);

            return model;
        }

        public async Task<int> GetAllUsersCountAsync()
        {
            var cnt = await this.AnimeContext.Users.CountAsync();

            return cnt;
        }

        public async Task<int> GetAllComplaintsCountAsync()
        {
            var cnt = await this.AnimeContext.Complaints.CountAsync();

            return cnt;
        }

        public async Task BanUserAsync(string userId,DateTime ban)
        {
            var user = await this.GetUserByIdAsync(userId);

            var date = DateTime.UtcNow;
            date = date.AddDays(ban.Subtract(date).Days);

            user.Ban = date;
            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task UnbanUserAsync(string username)
        {
            var user = await this.GetUserByUsernameAsync(username);

            user.Ban = new DateTime();
            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await this.AnimeContext.Comments.FindAsync(commentId);

            this.AnimeContext.Comments.Remove(comment);
            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task AddComplaint(ComplaintBindingModel complaintModel)
        {
            var complaint = this.Mapper.Map<Complaint>(complaintModel);

            await this.AnimeContext.Complaints.AddAsync(complaint);
            await this.AnimeContext.SaveChangesAsync();
        }

        public async Task RemoveComplaint(int complaintId)
        {
            var complaint = this.AnimeContext.Complaints.Find(complaintId);

            this.AnimeContext.Complaints.Remove(complaint);
            await this.AnimeContext.SaveChangesAsync();
        }
    }
}
