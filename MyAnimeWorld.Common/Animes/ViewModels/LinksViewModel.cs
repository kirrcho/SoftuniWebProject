using MyAnimeWorld.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Common.Animes.ViewModels
{
    public class LinksViewModel
    {
        public LinksViewModel()
        {
            this.Links = new List<AnimeLinkEnum>();
            this.LinksAlreadyAdded = new Dictionary<int, string>();
        }

        public int AnimeSeriesId { get; set; }

        public int EpisodeNumber { get; set; }

        public List<AnimeLinkEnum> Links { get; set; }

        public Dictionary<int,string> LinksAlreadyAdded { get; set; }
    }
}
