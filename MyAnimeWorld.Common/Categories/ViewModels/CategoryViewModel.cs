using MyAnimeWorld.Common.Main.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Common.Categories.ViewModels
{
    public class CategoryViewModel
    {
        public string Name { get; set; }

        public IEnumerable<AnimeSeriesViewModel> Animes { get; set; }
    }
}
