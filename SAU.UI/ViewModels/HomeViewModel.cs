using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using SAU.UI.Services.Contracts;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace SAU.UI.ViewModels
{
    public class HomeViewModel: ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly IWindowService _windowService;

        private ICommand _navigateCommand;

        private ICommand _openWindowCommand;

        public ICommand NavigateCommand => _navigateCommand ??= new RelayCommand<string>(OnNavigate);

        public HomeViewModel(INavigationService navigationService, IWindowService windowService)
        {
            _navigationService = navigationService;
            _windowService = windowService;
        }

        private void OnNavigate(string obj)
        {
            
        }

        public ICommand OpenWindowCommand => _openWindowCommand ??= new RelayCommand<string>(OnOpenWindow);

        private void OnOpenWindow(string obj)
        {
            
        }

        public void OnNavigatedTo()
        {
            System.Diagnostics.Debug.WriteLine($"INFO | {typeof(HomeViewModel)} navigated", "Wpf.Ui.Demo");
        }

        public void OnNavigatedFrom()
        {
            System.Diagnostics.Debug.WriteLine($"INFO | {typeof(HomeViewModel)} navigated", "Wpf.Ui.Demo");
        }
    }
}