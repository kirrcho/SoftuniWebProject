using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAnimeWorld.Models
{
    public class Category
    {
        public int Id { get; set; }

        public Category()
        {
            this.Animes = new List<AnimeSeriesCategories>();
        }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        public ICollection<AnimeSeriesCategories> Animes { get; set; }
    }
}
