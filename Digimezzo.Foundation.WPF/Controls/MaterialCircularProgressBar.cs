using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class MaterialCircularProgressBar : ProgressBar
    {
        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register(nameof(Accent), typeof(Brush), typeof(MaterialCircularProgressBar), new PropertyMetadata(Brushes.Red));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(MaterialCircularProgressBar), new PropertyMetadata(3.0));

        

        static MaterialCircularProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialCircularProgressBar), new FrameworkPropertyMetadata(typeof(MaterialCircularProgressBar)));
        }
    }
}
