using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SAU.UI.Models;
using SAU.UI.Services.Interfaces;
using Wpf.Ui.Appearance;

namespace SAU.UI.Services
{
    public class SettingsApp: ISettingsApp
    {
        private readonly string _filePath;

        public string Culture { get; set; } = "en-US";
        public string PathGame { get; set; }
        public ThemeType ThemeApp { get; set; } = ThemeType.Dark;
        public List<GitHubSource> GitHubSources { get; set; }

        public SettingsApp(string filePath)
        {
            _filePath = filePath;
        }

        public void Load()
        {
            try
            {
                var serializer = new JsonSerializer();
                
                if (!File.Exists(_filePath))
                    Save();

                using var fs = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(fs);

                serializer.Populate(reader, this);
            }
            catch (Exception)
            {
                // TODO: ошибка загрузки настроек
            }
            finally
            {
                SettingsLoaded?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Save()
        {
            try
            {
                var serializer = new JsonSerializer()
                {
                    Formatting = Formatting.Indented
                };

                using var fs = File.Open(_filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                using var writer = new StreamWriter(fs);

                serializer.Serialize(writer, this);

                SettingsSaved?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                SettingsSaved?.Invoke(this, EventArgs.Empty);

            }
        }

        public void Reset()
        {
            SettingsReset?.Invoke(this, EventArgs.Empty);
            Load();
        }

        public event EventHandler SettingsReset;
        public event EventHandler SettingsSaved;
        public event EventHandler SettingsLoaded;
    }
}