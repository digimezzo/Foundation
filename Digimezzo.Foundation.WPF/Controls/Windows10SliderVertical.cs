using System.Windows;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class Windows10SliderVertical : Windows8SliderVertical
    {
        static Windows10SliderVertical()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Windows10SliderVertical), new FrameworkPropertyMetadata(typeof(Windows10SliderVertical)));
        }
    }
}
