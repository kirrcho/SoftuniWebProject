using MyAnimeWorld.Common.Main.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Common.Users.ViewModels
{
    public class PagedUsersViewModel
    {
        public PagedUsersViewModel()
        {
            this.Users = new List<UserViewModel>();
            this.Pagination = new PagesViewModel();
        }

        public string SearchTerm { get; set; }

        public IEnumerable<UserViewModel> Users { get; set; }

        public PagesViewModel Pagination { get; set; }
    }
}
