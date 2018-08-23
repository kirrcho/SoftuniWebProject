using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAnimeWorld.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int EpisodeId { get; set; }

        public AnimeEpisode Episode { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
        
        [Required]
        [MinLength(5)]
        public string CommentContent { get; set; }

        [Required]
        public DateTime DateCreatedAt { get; set; }
    }
}
