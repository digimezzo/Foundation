using Digimezzo.Foundation.WPF.Base;
using System.Windows;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class FAIcon : FontIconBase
    {
        static FAIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FAIcon), new FrameworkPropertyMetadata(typeof(FAIcon)));
        }
    }

    public class SegoeIcon : FontIconBase
    {
        static SegoeIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SegoeIcon), new FrameworkPropertyMetadata(typeof(SegoeIcon)));
        }
    }

    public class MaterialIcon : FontIconBase
    {
        static MaterialIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialIcon), new FrameworkPropertyMetadata(typeof(MaterialIcon)));
        }
    }
}
