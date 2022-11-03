using CommunityToolkit.Mvvm.ComponentModel;
using SAU.UI.Models;
using System.Windows.Input;
using Wpf.Ui.Appearance;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace SAU.UI.ViewModels
{
    public class SettingViewModel: ObservableObject
    {
        private readonly INavigationService _navigationService;
        private ICommand _showMoreCommand;
        private readonly IThemeService _themeService;

        public bool ThemeApp
        {
            get { return _themeService.GetTheme() == ThemeType.Dark ? true : false; }

            set { _themeService.SetTheme(_themeService.GetTheme() == ThemeType.Dark ? ThemeType.Light : ThemeType.Dark); }
        }

        public SettingViewModel(INavigationService navigationService, IThemeService themeService)
        {
            _navigationService = navigationService;
            _themeService = themeService;

            var settings = _themeService;
        }
    }
}