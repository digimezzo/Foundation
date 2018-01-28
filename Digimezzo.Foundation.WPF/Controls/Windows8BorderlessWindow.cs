using Digimezzo.Foundation.WPF.Base;
using System.Windows;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class BorderlessWindows8Window : BorderlessWindowBase
    {
        static BorderlessWindows8Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BorderlessWindows8Window), new FrameworkPropertyMetadata(typeof(BorderlessWindows8Window)));
        }
    }
}
