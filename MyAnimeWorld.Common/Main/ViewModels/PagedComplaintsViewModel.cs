using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Common.Main.ViewModels
{
    public class PagedComplaintsViewModel
    {
        public PagedComplaintsViewModel()
        {
            this.Complaints = new List<ComplaintViewModel>();
            this.Pagination = new PagesViewModel();
        }

        public string SearchTerm { get; set; }

        public IEnumerable<ComplaintViewModel> Complaints { get; set; }

        public PagesViewModel Pagination { get; set; }
    }
}
