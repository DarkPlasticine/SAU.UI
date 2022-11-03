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

            Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/icon.png", UriKind.Absolute));

            ContextMenu = new ContextMenu
            {
                FontSize = 14d,
                Items =
                {
                    new Separator(),
                    new Wpf.Ui.Controls.MenuItem
                    {
                        Header = "Выход",
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
            System.Diagnostics.Debug.WriteLine($"DEBUG | WPF UI Tray event: {nameof(OnLeftClick)}", "Wpf.Ui.Demo");
        }

        private void OnMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem)
                return;

            System.Diagnostics.Debug.WriteLine($"DEBUG | WPF UI Tray clicked: {menuItem.Tag}", "Wpf.Ui.Demo");

            switch (menuItem.Tag)
            {
                case "exitApp": App.Current.Shutdown(0); break;
                    default: break;
            }
        }
    }
}