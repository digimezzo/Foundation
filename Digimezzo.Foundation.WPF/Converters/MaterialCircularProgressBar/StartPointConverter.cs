using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Digimezzo.Foundation.WPF.Converters.MaterialCircularProgressBar
{
    /// <summary>
    /// This code is based on code from MaterialDesignInXamlToolkit: https://github.com/ButchersBoy/MaterialDesignInXamlToolkit
    /// Their license is included in the "Licenses" folder.
    /// </summary>
    public class StartPointConverter : IValueConverter
    {
        [Obsolete]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double && ((double)value > 0.0))
            {
                return new Point((double)value / 2, 0);
            }

            return new Point();
        }

        [Obsolete]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
