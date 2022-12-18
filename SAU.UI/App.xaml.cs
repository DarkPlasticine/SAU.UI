using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SAU.UI.Models;
using SAU.UI.Services;
using SAU.UI.Services.Contracts;
using SAU.UI.Services.Interfaces;
using SAU.UI.ViewModels;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace SAU.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c =>
            {
                c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location));
            })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // Taskbar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Snackbar service
                services.AddSingleton<ISnackbarService, SnackbarService>();

                // Dialog service
                services.AddSingleton<IDialogService, DialogService>();

                // Tray icon
                services.AddSingleton<INotifyIconService, CustomNotifyIconService>();

                // Page resolver service
                services.AddSingleton<IPageService, PageService>();

                // Page resolver service
                services.AddSingleton<IWindowService, WindowService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                services.AddSingleton<ISettingsApp>(x => new SettingsApp(Path.Combine(Environment.CurrentDirectory, @"settings.sau")));

                // Main window container with navigation
                services.AddScoped<INavigationWindow, Views.Container>();
                services.AddScoped<ContainerViewModel>();

                services.AddScoped<Views.Pages.Home>();
                services.AddScoped<HomeViewModel>();

                services.AddScoped<Views.Pages.SearchAddon>();
                services.AddScoped<SearchAddonViewModel>();

                services.AddScoped<Views.Pages.Setting>();
                services.AddScoped<SettingViewModel>();

                services.AddScoped<Views.Pages.News>();
                services.AddScoped<NewsViewModel>();

                services.AddScoped<Views.Pages.About>();
                

                // Configuration
                services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));

            })
            .Build();


        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            LoadSettings();

            await _host.StartAsync();
        }

        private void LoadSettings()
        {
            var settings = GetService<ISettingsApp>();

            settings.Load();

            var resDic = new ResourceDictionary();
            resDic.Source = new Uri($@"pack://application:,,,/Resources/Languages/{settings.Culture}.xaml", UriKind.RelativeOrAbsolute);

            App.Current.Resources.MergedDictionaries.Add(resDic);

            GetService<IThemeService>().SetTheme(settings.ThemeApp);
        }
    }
}
