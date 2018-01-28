using Digimezzo.Foundation.WPF.Base;
using System.Windows;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class MaterialFloatingActionButton : MaterialButtonBase
    {
        static MaterialFloatingActionButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialFloatingActionButton), new FrameworkPropertyMetadata(typeof(MaterialFloatingActionButton)));
        }
    }
}
