using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAnimeWorld.Models
{
    public class AnimeSeries
    {
        public AnimeSeries()
        {
            this.UsersFavouriteAnime = new List<UserRatedAnime>();
            this.Categories = new List<AnimeSeriesCategories>();
            this.Episodes = new List<AnimeEpisode>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Title { get; set; }

        [Required]
        [MinLength(30)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Required]
        public DateTime DateCreatedAt { get; set; }

        public ICollection<AnimeSeriesCategories> Categories { get; set; }

        public ICollection<AnimeEpisode> Episodes { get; set; }

        public ICollection<UserRatedAnime> UsersFavouriteAnime { get; set; }
    }
}
