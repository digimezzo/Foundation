using System;
using System.Globalization;
using System.Windows.Data;

namespace Digimezzo.Foundation.WPF.Converters.MaterialCircularProgressBar
{
    /// <summary>
    /// This code is based on code from MaterialDesignInXamlToolkit: https://github.com/ButchersBoy/MaterialDesignInXamlToolkit
    /// Their license is included in the "Licenses" folder.
    /// </summary>
    public class NotZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse((value ?? "").ToString(), out double val))
            {
                return Math.Abs(val) > 0.0;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
