using System.Collections.Generic;
using System.Linq;

namespace Digimezzo.Foundation.WPF.Converters.MaterialCircularProgressBar
{
    /// <summary>
    /// This code is based on code from MaterialDesignInXamlToolkit: https://github.com/ButchersBoy/MaterialDesignInXamlToolkit
    /// Their license is included in the "Licenses" folder.
    /// </summary>
    internal static class LocalEx
    {
        public static double ExtractDouble(this object val)
        {
            var d = val as double? ?? double.NaN;
            return double.IsInfinity(d) ? double.NaN : d;
        }


        public static bool AnyNan(this IEnumerable<double> vals)
        {
            return vals.Any(double.IsNaN);
        }
    }
}
