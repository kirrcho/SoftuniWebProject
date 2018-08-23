using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAnimeWorld.Models
{
    public class AnimeEpisode
    {
        public AnimeEpisode()
        {
            this.Links = new List<AnimeLink>();
            this.Comments = new List<Comment>();
        }

        public int Id { get; set; }

        public int AnimeSeriesId { get; set; }

        public AnimeSeries AnimeSeries { get; set; }

        [Required]
        public int EpisodeNumber { get; set; }

        [Required]
        public DateTime DateCreatedAt { get; set; }

        public ICollection<AnimeLink> Links { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
