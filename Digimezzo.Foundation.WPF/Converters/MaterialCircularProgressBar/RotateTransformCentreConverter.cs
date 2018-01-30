using System;
using System.Globalization;
using System.Windows.Data;

namespace Digimezzo.Foundation.WPF.Converters.MaterialCircularProgressBar
{
    /// <summary>
    /// This code is based on code from MaterialDesignInXamlToolkit: https://github.com/ButchersBoy/MaterialDesignInXamlToolkit
    /// Their license is included in the "Licenses" folder.
    /// </summary>
    public class RotateTransformCentreConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // value == actual width
            return (double)value / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
