using System;
using System.IO;

namespace SAU.UI.Models
{
    public class Addon
    {
        public string Title { get; set; }
        public DirectoryInfo Info { get; set; }
        public bool NeedUpdate { get; set; }
        public DateOnly LastUpdate { get; set; }
        public Toc AddonInfo { get; set; }
    }
}