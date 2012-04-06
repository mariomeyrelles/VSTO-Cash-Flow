using System;
using System.Windows;
using System.Windows.Data;

namespace ModernCashFlow.WpfTests
{

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class HasErrorToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool hasError = (bool)value;
            return hasError ? Visibility.Visible : Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}