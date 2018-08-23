using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Common.Animes.ViewModels
{
    public class AlternativeLinkViewModel
    {
        public int LinkId { get; set; }

        public string Source { get; set; }

        public string LinkName { get; set; }

        public bool LinkSelected { get; set; }

        public int EpisodeId { get; set; }
    }
}
