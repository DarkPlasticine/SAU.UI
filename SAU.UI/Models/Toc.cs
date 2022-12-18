using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace SAU.UI.Models
{
    public class Toc
    {
        public string Version { get; set; }
        public string Author { get; set; }

        public Toc(string filePath)
        {
            using (var fs = new StreamReader(filePath))
            {
                string line;

                while ((line = fs.ReadLine()) != null)
                {
                    if (line.StartsWith("## Version", StringComparison.OrdinalIgnoreCase))
                        Version = line.Replace("## Version", "").Replace(":", "").Trim();

                    if (line.StartsWith("## Author", StringComparison.OrdinalIgnoreCase))
                        Author = line.Replace("## Author", "").Replace(":", "").Trim();

                }
            }
        }
    }
}