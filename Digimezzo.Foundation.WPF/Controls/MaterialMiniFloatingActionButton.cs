using Digimezzo.Foundation.WPF.Base;
using System.Windows;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class MaterialMiniFloatingActionButton : MaterialButtonBase
    {
        static MaterialMiniFloatingActionButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialMiniFloatingActionButton), new FrameworkPropertyMetadata(typeof(MaterialMiniFloatingActionButton)));
        }
    }
}
