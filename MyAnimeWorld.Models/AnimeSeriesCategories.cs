using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Models
{
    public class AnimeSeriesCategories
    {
        public int AnimeId { get; set; }

        public AnimeSeries AnimeSeries { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
