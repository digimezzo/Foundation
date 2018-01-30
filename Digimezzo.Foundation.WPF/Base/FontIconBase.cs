using System.Windows;
using System.Windows.Controls;

namespace Digimezzo.Foundation.WPF.Base
{
    public class FontIconBase : Control
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(FontIconBase), new PropertyMetadata(null));

        static FontIconBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FontIconBase), new FrameworkPropertyMetadata(typeof(FontIconBase)));
        }
    }
}
