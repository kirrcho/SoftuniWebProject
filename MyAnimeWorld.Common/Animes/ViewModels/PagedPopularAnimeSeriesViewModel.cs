using MyAnimeWorld.Common.Main.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Common.Animes.ViewModels
{
    public class PagedPopularAnimeSeriesViewModel
    {
        public PagedPopularAnimeSeriesViewModel()
        {
            this.Animes = new List<PopularAnimeSeriesViewModel>();
            this.Pagination = new PagesViewModel();
        }

        public ICollection<PopularAnimeSeriesViewModel> Animes { get; set; }

        public PagesViewModel Pagination { get; set; }
    }
}
