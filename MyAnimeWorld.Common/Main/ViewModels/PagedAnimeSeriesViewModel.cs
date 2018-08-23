using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Common.Main.ViewModels
{
    public class PagedAnimeSeriesViewModel
    {
        public PagedAnimeSeriesViewModel()
        {
            this.Animes = new List<AnimeSeriesViewModel>();
            this.Pagination = new PagesViewModel();
        }

        public ICollection<AnimeSeriesViewModel> Animes { get; set; }

        public PagesViewModel Pagination { get; set; }
    }
}
