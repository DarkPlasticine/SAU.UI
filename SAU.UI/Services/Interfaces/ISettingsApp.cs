using System;
using Wpf.Ui.Appearance;

namespace SAU.UI.Services.Interfaces
{
    public interface ISettingsApp
    {
        string Culture { get; set; }
        string PathGame { get; set; }
        ThemeType ThemeApp { get; set; }
        void Load();
        void Save();
        void Reset();
        event EventHandler? SettingsReset;
        event EventHandler? SettingsSaved;
        event EventHandler? SettingsLoaded;
        
    }
}