using System.Windows;
using System.Windows.Controls;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class LabelToUpper : Label
    {
        static LabelToUpper()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelToUpper), new FrameworkPropertyMetadata(typeof(LabelToUpper)));
        }
    }
}
