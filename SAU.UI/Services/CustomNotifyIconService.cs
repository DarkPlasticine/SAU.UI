using System;
using System.Windows.Media.Imaging;
using Wpf.Ui.Mvvm.Services;

namespace SAU.UI.Services
{
    public class CustomNotifyIconService: NotifyIconService
    {
        public CustomNotifyIconService()
        {
            TooltipText = "Sirus Addon Updater";

            Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/alrosa-gr.png", UriKind.Absolute));
        }
    }
}