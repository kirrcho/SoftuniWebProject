using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Common.Main.ViewModels
{
    public class PagedAnimesViewModel
    {
        public PagedAnimesViewModel()
        {
            this.Animes = new List<AnimeViewModel>();
            this.Pagination = new PagesViewModel();
        }

        public ICollection<AnimeViewModel> Animes { get; set; }

        public PagesViewModel Pagination { get; set; }
    }
}
