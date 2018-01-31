using Digimezzo.Foundation.WPF.Base;
using System.Windows;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class AwesomeIcon : FontIconBase
    {
        static AwesomeIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AwesomeIcon), new FrameworkPropertyMetadata(typeof(AwesomeIcon)));
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
