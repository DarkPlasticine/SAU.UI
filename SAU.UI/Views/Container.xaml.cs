using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SAU.UI.Models;
using SAU.UI.ViewModels;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.TaskBar;

namespace SAU.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Container : INavigationWindow
    {
        private bool _initalize = false;

        private readonly IThemeService _themeService;
        private readonly ITaskBarService _taskBarService;

        public ContainerViewModel ViewModel { get; }

        public Container(ContainerViewModel viewModel, 
            INavigationService navigationService, 
            IPageService pageService, 
            IThemeService themeService, 
            ITaskBarService taskBarService, 
            ISnackbarService snackbarService, 
            IDialogService dialogService)
        {
            ViewModel = viewModel;
            DataContext = this;

            _themeService = themeService;
            _taskBarService = taskBarService;

            InitializeComponent();

            SetPageService(pageService);

            navigationService.SetNavigationControl(RootNavigation);
            dialogService.SetDialogControl(RootDialog);

            Loaded += (_, _) => InvokeSplashScreen();
        }

        private void InvokeSplashScreen()
        {
            if (_initalize)
                return;

            _initalize = true;

            ContainerGrid.Visibility = Visibility.Collapsed;
            RootWelcomeGrid.Visibility = Visibility.Visible;

            _taskBarService.SetState(this, TaskBarProgressState.Indeterminate);

            Task.Run(async () =>
            {
                await Task.Delay(4000);

                await Dispatcher.InvokeAsync(() =>
                {
                    RootWelcomeGrid.Visibility = Visibility.Hidden;
                    ContainerGrid.Visibility = Visibility.Visible;

                    Navigate(typeof(Pages.Home));

                    _taskBarService.SetState(this, TaskBarProgressState.None);
                });

                return true;
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }

        #region INavigationWindow methods

        public Frame GetFrame() => RootFrame;

        public INavigation GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) => RootNavigation.PageService = pageService;

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        #endregion

        private void RootNavigation_OnNavigated(INavigation sender, RoutedNavigationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"DEBUG | SAU UI Navigated to: {sender?.Current ?? null}", "SAU.UI");

            RootFrame.Margin = new Thickness(left: 0,
                top: sender?.Current?.PageTag == "home" ? -69 : 0,
                right: 0,
                bottom: 0);
        }
    }
}
