using Digimezzo.Foundation.WPF.Base;
using System.Windows;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class MaterialRaisedButton : MaterialButtonBase
    {
        static MaterialRaisedButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialRaisedButton), new FrameworkPropertyMetadata(typeof(MaterialRaisedButton)));
        }
    }
}
