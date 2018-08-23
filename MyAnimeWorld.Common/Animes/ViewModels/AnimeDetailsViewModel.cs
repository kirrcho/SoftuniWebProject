using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Common.Animes.ViewModels
{
    public class AnimeDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int Rating { get; set; }

        public bool IsFavourite { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public Dictionary<int,int> Episodes { get; set; }
    }
}
