using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Common.Animes.ViewModels
{
    public class WatchEpisodeViewModel
    {
        public WatchEpisodeViewModel()
        {
            this.Links = new List<AlternativeLinkViewModel>();
        }

        public int NextEpisodeId { get; set; }

        public int PreviousEpisodeId { get; set; }

        public IEnumerable<AlternativeLinkViewModel> Links { get; set; }
    }
}
