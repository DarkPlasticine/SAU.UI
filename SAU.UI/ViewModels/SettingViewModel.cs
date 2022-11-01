using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using Wpf.Ui.Mvvm.Contracts;

namespace SAU.UI.ViewModels
{
    public class SettingViewModel: ObservableObject
    {
        private readonly INavigationService _navigationService;
        private ICommand _showMoreCommand;

        public SettingViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}