using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using SAU.UI.Models;
using System.Windows.Input;
//using CommunityToolkit.Mvvm.Input;
using SAU.UI.Views.Pages;
using Wpf.Ui.Appearance;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using CommunityToolkit.Mvvm.Input;
using SAU.UI.Services.Interfaces;
using SAU.UI.Services;

namespace SAU.UI.ViewModels
{
    public partial class SettingViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IThemeService _themeService;
        private readonly ISnackbarService _snackbarService;
        private readonly IDialogService _dialogService;
        private readonly ISettingsApp _settingsApp;

        //private ICommand _selectFolderCommand;
        //private ICommand _changeLangCommand;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ShowFolderDialogCommand))]
        private string _gameFolder;
        
        [ObservableProperty]
        private string _selectedThemeApp;

        private int _selectedLanguage = 0;

        [ObservableProperty]
        private IEnumerable<CultureInfo> _languageList = new CultureInfo[]{};

        //public ICommand SelectFolderCommand => _selectFolderCommand ??= new RelayCommand(ShowFolderDialog);
        //public ICommand ChangeLangCommand => _changeLangCommand ??= new RelayCommand(ChangeLanguage);

        public bool ThemeApp
        {
            get { return _themeService.GetTheme() == ThemeType.Dark; }

            set
            {
                SelectedThemeApp = value
                    ? Application.Current.Resources["tbParamThemeDark"].ToString()
                    : Application.Current.Resources["tbParamThemeLight"].ToString();

                _themeService.SetTheme(_themeService.GetTheme() == ThemeType.Dark ? ThemeType.Light : ThemeType.Dark);
                _settingsApp.ThemeApp = _themeService.GetTheme();
                _settingsApp.Save();
            }
        }


        /// <summary>
        /// Выбранная тема
        /// </summary>
        //public string SelectedThemeApp
        //{
        //    get => _selectedThemeApp;
        //    set => SetProperty(ref _selectedThemeApp, value);
        //}

        /// <summary>
        /// Язык приложения
        /// </summary>
        public int SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                SetProperty(ref _selectedLanguage, value);
                ChangeLanguageCommand.Execute(value);
            }
        }

        //public IEnumerable<CultureInfo> LanguageList
        //{
        //    get => _languageList;
        //    set => SetProperty(ref _languageList, value);
        //}

        public SettingViewModel()
        {
            _navigationService = App.GetService<INavigationService>();
            _themeService = App.GetService<IThemeService>();
            _snackbarService = App.GetService<ISnackbarService>();
            _dialogService = App.GetService<IDialogService>();
            _settingsApp = App.GetService<ISettingsApp>();

            _settingsApp.SettingsSaved += _settingsApp_SettingsSaved;

            SelectedThemeApp = _settingsApp.ThemeApp == ThemeType.Dark ? Application.Current.Resources["tbParamThemeDark"].ToString() : Application.Current.Resources["tbParamThemeLight"].ToString();

            LanguageList = new[]
            {
                CultureInfo.GetCultureInfo("ru-RU"),
                CultureInfo.GetCultureInfo("en-US")
            };

            _selectedLanguage = LanguageList.ToList().FindIndex(f => f.Name.Equals(_settingsApp.Culture));
            _gameFolder = _settingsApp.PathGame;
        }

        private void _settingsApp_SettingsSaved(object sender, EventArgs e)
        {
            ShowSnackbar();
        }

        [RelayCommand]
        private void ShowFolderDialog()
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

            if (dialog.ShowDialog() == true)
            {               
                GameFolder = dialog.SelectedPath;
                _settingsApp.PathGame = dialog.SelectedPath;
                _settingsApp.Save();
            }
        }

        /// <summary>
        /// <see cref="https://github.com/Epsil0neR/WPF-Localization/"/>
        /// </summary>
        [RelayCommand]
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

            _settingsApp.Culture = selectLang.Name;
            _settingsApp.Save();
        }

        private void ShowSnackbar()
        {
            _snackbarService.Show(Application.Current.Resources["MenuItemSettings"].ToString(),
                Application.Current.Resources["msgSaveSettings"].ToString(), 
                SymbolRegular.Settings24, ControlAppearance.Primary);
        }
    }
}