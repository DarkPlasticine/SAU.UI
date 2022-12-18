using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using SAU.UI.Services.Contracts;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using SAU.UI.Services.Interfaces;
using Wpf.Ui.Controls.Interfaces;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using SAU.UI.Models;

namespace SAU.UI.ViewModels
{
    public partial class HomeViewModel: ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IWindowService _windowService;
        private readonly ISettingsApp _settingsApp;
        private readonly ISnackbarService _snackbarService;
        private readonly IDialogService _dialogService;
        private readonly IDialogControl _dialogControl;

        private string _addonPath = $"Interface\\AddOns\\";
        private string _filter;

        private ICommand _reloadInstallAddonCommand;

        private ICollectionView _installedAddons => CollectionViewSource.GetDefaultView(InstalledAddons);

        
        public ObservableCollection<Addon> InstalledAddons { get; private set; }

        public string Filter
        {
            get => _filter;
            set
            {
                if (value != _filter)
                {
                    SetProperty(ref _filter, value);
                    _installedAddons.Refresh();
                }
            }
        }

        public HomeViewModel(INavigationService navigationService, IWindowService windowService)
        {
            _navigationService = navigationService;
            _windowService = windowService;
            _settingsApp = App.GetService<ISettingsApp>();
            _snackbarService = App.GetService<ISnackbarService>();
            _dialogService = App.GetService<IDialogService>();
            _dialogControl = App.GetService<IDialogControl>();

            if (string.IsNullOrEmpty(_settingsApp.PathGame))
                _dialogControl.Show("Предупреждение", "Укажите путь к игре в настройках!");
            else
            {
                OnReloadInstallAddon(null);
                //_installedAddons.Filter += OnFiltering;
                _installedAddons.Filter = o => string.IsNullOrEmpty(Filter) || ((Addon) o).Title.Contains(Filter, StringComparison.OrdinalIgnoreCase);
            }
                
        }

        public ICommand ReloadInstallAddonCommand => _reloadInstallAddonCommand ??= new RelayCommand<string>(OnReloadInstallAddon);

        private void OnReloadInstallAddon(string obj)
        {
            string path = Path.Combine(_settingsApp.PathGame, _addonPath);

            if (Directory.Exists(path))
            {
                var folders = Directory.GetDirectories(path).Where(w => !w.Contains("Blizzard_"));

                InstalledAddons = new ObservableCollection<Addon>();

                foreach (var folder in folders)
                {
                    var di = new DirectoryInfo(folder);

                    InstalledAddons.Add(new Addon
                    {
                        Info = di,
                        Title = di.Name,
                        LastUpdate = DateOnly.Parse(di.CreationTime.ToShortDateString()),
                        NeedUpdate = false,
                        AddonInfo = new Toc(Path.Combine(di.FullName, $"{di.Name}.toc"))
                    });
                }
            }
        }
    }
}