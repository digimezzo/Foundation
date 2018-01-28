using System.Windows;
using System.Windows.Controls;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class LabelToLower : Label
    {
        static LabelToLower()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelToLower), new FrameworkPropertyMetadata(typeof(LabelToLower)));
        }
    }
}
