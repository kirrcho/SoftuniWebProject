using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Common.Main.ViewModels
{
    public class PagesViewModel
    {
        public PagesViewModel()
        {
            this.Pages = new List<int>();
        }

        public string PageUrl { get; set; }

        public int CurrentPage { get; set; }

        public List<int> Pages { get; set; }
    }
}
