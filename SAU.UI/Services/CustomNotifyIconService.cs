using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Services;

namespace SAU.UI.Services
{
    public class CustomNotifyIconService: NotifyIconService
    {
        public CustomNotifyIconService()
        {
            TooltipText = "Sirus Addon Updater";

            Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/puzzle.png", UriKind.Absolute));

            ContextMenu = new ContextMenu
            {
                FontSize = 14d,
                Items =
                {
                    new Wpf.Ui.Controls.MenuItem
                    {
                        Header = "Open",
                        SymbolIcon = SymbolRegular.Window28,
                        Tag = "openApp"
                    },
                    new Separator(),
                    new Wpf.Ui.Controls.MenuItem
                    {
                        Header = Application.Current.Resources["TrayMenuItemExit"].ToString(),
                        SymbolIcon = SymbolRegular.ArrowExit20,
                        Tag = "exitApp"
                    }
                }
            };

            foreach (var item in ContextMenu.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += OnMenuItemClick;
            }
        }

        protected override void OnLeftClick()
        {
            System.Diagnostics.Debug.WriteLine($"DEBUG | SAU.UI Tray event: {nameof(OnLeftClick)}", "SAU.Ui");
        }

        private void OnMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem)
                return;

            System.Diagnostics.Debug.WriteLine($"DEBUG | SAU.Ui Tray clicked: {menuItem.Tag}", "SAU.Ui");

            switch (menuItem.Tag)
            {
                case "exitApp": App.Current.Shutdown(0); break;
                case "openApp": App.Current.MainWindow.WindowState = WindowState.Normal;
                    break;
                    default: break;
            }
        }
    }
}