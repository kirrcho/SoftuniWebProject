using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAnimeWorld.Common.Users.BindingModels;
using MyAnimeWorld.Common.Utilities.Constants;
using MyAnimeWorld.Models;
using MyAnimeWorld.Services;

namespace MyAnimeWorld.App.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "User")]
    public class ProfileModel : PageModel
    {
        private readonly UserService userService;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public ProfileModel(UserService userService,SignInManager<User> signInManager,UserManager<User> userManager)
        {
            this.userService = userService;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public string UserId { get; set; }

        public string Username { get; set; }

        public string AvatarUrl { get; set; }

        public string Email { get; set; }

        public string DateCreatedAt { get; set; }

        [DefaultValue(false)]
        public bool IsBanned { get; set; }

        [DefaultValue(false)]
        public bool IsAdmin { get; set; }

        [DefaultValue(false)]
        public bool CurrentUserProfile { get; set; }

        [BindProperty]
        public DateTime Ban { get; set; }

        public async Task OnGet(string username)
        {
            User user;
            string id = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (username == null)
            {
                user = await this.userService.GetUserByIdAsync(id);
            }
            else
            {
                user = await this.userService.GetUserByUsernameAsync(username);
            }

            //The current page is the user's own profile
            if (user.Id == id)
            {
                this.CurrentUserProfile = true;
            }
            if (this.User.IsInRole(DbConstants.Admin_Role))
            {
                this.IsAdmin = true;
            }
            if (DateTime.Compare(user.Ban,DateTime.UtcNow) >= 0)
            {
                this.IsBanned = true;
            }

            this.LoadProperties(user);
        }

        public async Task<IActionResult> OnPost(string username)
        {
            User user = await this.userService.GetUserByUsernameAsync(username);

            if (DateTime.Compare(this.Ban,DateTime.UtcNow) <= 0)
            {
                this.LoadProperties(user);

                this.TempData.Add(ErrorConstants.Error_Key, ErrorConstants.Invalid_Date);
                return this.Page();
            }

            await this.userService.BanUserAsync(user.Id, this.Ban);
            await this.userManager.UpdateSecurityStampAsync(user);



            //new DateTime.UtcNow creates a 23:59:59 .. day that doesn't add up
            var daysBanned = this.Ban.Subtract(DateTime.UtcNow).Days + 1;

            if (daysBanned == 1)
            {
                this.TempData["message"] = $"User was banned successfully for 1 day!";
            }
            else
            {
                this.TempData["message"] = $"User was banned successfully for {daysBanned} days!";
            }

            return this.Redirect("/admin/deleteseries/success");
        }

        public async Task<IActionResult> OnPostUnbanUser(string username)
        {
            await this.userService.UnbanUserAsync(username);

            this.TempData["message"] = "User was successfully unbanned.";

            return this.Redirect("/admin/deleteseries/success");
        }

        private void LoadProperties(User user)
        {
            this.Username = user.UserName;
            this.AvatarUrl = user.AvatarUrl;
            this.Email = user.Email;
            this.DateCreatedAt = user.DateCreatedAt.ToString("dd\\/MMM\\/yyyy");
            this.UserId = user.Id;
        }
    }
}