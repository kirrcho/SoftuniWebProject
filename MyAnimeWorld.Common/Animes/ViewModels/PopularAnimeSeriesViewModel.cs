using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Common.Animes.ViewModels
{
    public class PopularAnimeSeriesViewModel
    {
        public int AnimeSeriesId { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public decimal Rating { get; set; }
    }
}
