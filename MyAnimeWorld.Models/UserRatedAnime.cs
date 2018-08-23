using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MyAnimeWorld.Models
{
    public class UserRatedAnime
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public int AnimeId { get; set; }

        public AnimeSeries AnimeSeries { get; set; }

        [DefaultValue(false)]
        public bool IsFavourite { get; set; }

        [DefaultValue(0)]
        public int Rating { get; set; }
    }
}
