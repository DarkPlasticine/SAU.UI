using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using SAU.UI.Services.Interfaces;

namespace SAU.UI.Services
{
    public class DateConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var settings = App.GetService<ISettingsApp>();
            return ((DateOnly) value).ToString(CultureInfo.GetCultureInfo(settings.Culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}