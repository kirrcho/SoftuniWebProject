using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Common.Animes.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int EpisodeId { get; set; }

        public string Username { get; set; }

        public string Content { get; set; }

        public string AvatarUrl { get; set; }

        public DateTime DateCreatedAt { get; set; }
    }
}
