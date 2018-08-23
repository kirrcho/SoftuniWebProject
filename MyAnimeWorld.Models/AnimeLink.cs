using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAnimeWorld.Models
{
    public class AnimeLink
    {
        public int Id { get; set; }

        public int AnimeId { get; set; }

        public AnimeSeries Anime { get; set; }

        public int SourceId { get; set; }

        [Required]
        public AnimeLinkEnum Source { get; set; }

        public int EpisodeId { get; set; }

        public AnimeEpisode Episode { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string SourceUrl { get; set; }
    }
}
