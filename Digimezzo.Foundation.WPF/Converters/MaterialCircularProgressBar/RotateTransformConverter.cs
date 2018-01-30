using System;
using System.Globalization;
using System.Windows.Data;

namespace Digimezzo.Foundation.WPF.Converters.MaterialCircularProgressBar
{
    /// <summary>
    /// This code is based on code from MaterialDesignInXamlToolkit: https://github.com/ButchersBoy/MaterialDesignInXamlToolkit
    /// Their license is included in the "Licenses" folder.
    /// </summary>
    public class RotateTransformConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value = values[0].ExtractDouble();
            var minimum = values[1].ExtractDouble();
            var maximum = values[2].ExtractDouble();

            if (new[] { value, minimum, maximum }.AnyNan())
                return Binding.DoNothing;

            var percent = maximum <= minimum ? 1.0 : (value - minimum) / (maximum - minimum);

            return 360 * percent;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
