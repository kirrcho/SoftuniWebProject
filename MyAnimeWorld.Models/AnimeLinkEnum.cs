using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAnimeWorld.Models
{
    public class AnimeLinkEnum
    {
        public AnimeLinkEnum()
        {
            this.AnimeLinks = new List<AnimeLink>();
        }

        public int Id { get; set; }

        [Required]  
        public string Name { get; set; }

        public ICollection<AnimeLink> AnimeLinks { get; set; }
    }
}
