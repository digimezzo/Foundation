using Digimezzo.Foundation.WPF.Base;
using System.Windows;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class MaterialFlatButton : MaterialButtonBase
    {
        static MaterialFlatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialFlatButton), new FrameworkPropertyMetadata(typeof(MaterialFlatButton)));
        }
    }
}
