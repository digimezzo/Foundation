using System.Windows;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class Windows10SliderHorizontal : Windows8SliderHorizontal
    {
        static Windows10SliderHorizontal()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Windows10SliderHorizontal), new FrameworkPropertyMetadata(typeof(Windows10SliderHorizontal)));
        }
    }
}
