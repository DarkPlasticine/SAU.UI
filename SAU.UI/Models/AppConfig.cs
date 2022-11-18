using System;
using System.Globalization;
using System.IO;

namespace SAU.UI.Models
{
    public class AppConfig
    {
        /// <summary>
        /// Путь до папки с настройками
        /// </summary>
        public string ConfigurationsFolder { get; set; } = Path.Combine(Environment.CurrentDirectory, @"settings\settings.sau");
        public string AppPropertiesFileName { get; set; }
    }
}