using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using SAU.UI.Models;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SAU.UI.Views.Pages;
using Wpf.Ui.Appearance;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace SAU.UI.ViewModels
{
    public class SettingViewModel: ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IThemeService _themeService;

        private ICommand _selectFolderCommand;
        private ICommand _changeLangCommand;
        private string _gameFolder;
        private string _selectedThemeApp;
        private int _selectedLanguage = 0;
        private IEnumerable<CultureInfo> _languageList = new CultureInfo[]{};

        public ICommand SelectFolderCommand => _selectFolderCommand ??= new RelayCommand(ShowFolderDialog);
        public ICommand ChangeLangCommand => _changeLangCommand ??= new RelayCommand(ChangeLanguage);

        public bool ThemeApp
        {
            get { return _themeService.GetTheme() == ThemeType.Dark; }

            set
            {
                SelectedThemeApp = value
                    ? Application.Current.Resources["tbParamThemeDark"].ToString()
                    : Application.Current.Resources["tbParamThemeLight"].ToString();

                _themeService.SetTheme(_themeService.GetTheme() == ThemeType.Dark ? ThemeType.Light : ThemeType.Dark);
                
                //SelectedThemeApp = _themeService.GetTheme() == ThemeType.Dark
                //    ? Application.Current.Resources["tbParamThemeDark"].ToString()
                //    : Application.Current.Resources["tbParamThemeLight"].ToString();
            }
        }

        /// <summary>
        /// Папка игры
        /// </summary>
        public string GameFolder
        {
            get => _gameFolder;
            set => SetProperty(ref _gameFolder, value);
        }

        /// <summary>
        /// Выбранная тема
        /// </summary>
        public string SelectedThemeApp
        {
            get => _selectedThemeApp;
            set => SetProperty(ref _selectedThemeApp, value);
        }

        /// <summary>
        /// Язык приложения
        /// </summary>
        public int SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                SetProperty(ref _selectedLanguage, value);
                ChangeLangCommand.Execute(value);
            }
        }

        public IEnumerable<CultureInfo> LanguageList
        {
            get => _languageList;
            set => SetProperty(ref _languageList, value);
        }

        public SettingViewModel(INavigationService navigationService, IThemeService themeService)
        {
            _navigationService = navigationService;
            _themeService = themeService;
            SelectedThemeApp = _themeService.GetTheme() == ThemeType.Dark ? Application.Current.Resources["tbParamThemeDark"].ToString() : Application.Current.Resources["tbParamThemeLight"].ToString();

            _languageList = new[]
            {
                CultureInfo.GetCultureInfo("ru-RU"),
                CultureInfo.GetCultureInfo("en-US")
            };
        }

        private void ShowFolderDialog()
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

            if (dialog.ShowDialog() == true)
            {
                GameFolder = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// <see cref="https://github.com/Epsil0neR/WPF-Localization/"/>
        /// </summary>
        private void ChangeLanguage()
        {
            var selectLang = LanguageList.ElementAt(SelectedLanguage);

            var newLang = new ResourceDictionary()
            {
                Source = new Uri($"pack://application:,,,/Resources/Languages/{selectLang.Name}.xaml", UriKind.Absolute)
            };

            var oldLang = (from d in Application.Current.Resources.MergedDictionaries
                where d.Source != null && d.Source.OriginalString.Contains("Languages")
                select d).First();

            if (oldLang != null)
            {
                int id = Application.Current.Resources.MergedDictionaries.IndexOf(oldLang);
                Application.Current.Resources.MergedDictionaries.Remove(oldLang);
                Application.Current.Resources.MergedDictionaries.Insert(id, newLang);
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(newLang);
            }

            SelectedThemeApp = ThemeApp
                ? Application.Current.Resources["tbParamThemeDark"].ToString()
                : Application.Current.Resources["tbParamThemeLight"].ToString();
        }
    }
}