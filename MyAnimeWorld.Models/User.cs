using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyAnimeWorld.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            this.UserRatedAnimes = new List<UserRatedAnime>();
            this.Comments = new List<Comment>();
        }

        public string AvatarUrl { get; set; }

        public DateTime DateCreatedAt { get; set; }

        public ICollection<UserRatedAnime> UserRatedAnimes { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public DateTime Ban { get; set; }
    }
}
