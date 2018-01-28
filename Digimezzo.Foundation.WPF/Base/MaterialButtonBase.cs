using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Digimezzo.Foundation.WPF.Base
{
    public class MaterialButtonBase : Button
    {
        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register(nameof(Accent), typeof(Brush), typeof(MaterialButtonBase), new PropertyMetadata(Brushes.Red));

        static MaterialButtonBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialButtonBase), new FrameworkPropertyMetadata(typeof(MaterialButtonBase)));
        }
    }
}
