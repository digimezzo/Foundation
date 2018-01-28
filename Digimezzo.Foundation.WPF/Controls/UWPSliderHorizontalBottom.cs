using Digimezzo.Foundation.WPF.Base;
using System;
using System.Windows;
using System.Windows.Media;


namespace Digimezzo.Foundation.WPF.Controls
{
    public class UWPSliderHorizontalBottom : UWPSliderHorizontal
    {
        private static readonly CornerRadius rightCornerRadius = new CornerRadius(Constants.UWPSliderBaseUnit, Constants.UWPSliderBaseUnit, 0, Constants.UWPSliderBaseUnit);
        private static readonly CornerRadius leftCornerRadius = new CornerRadius(Constants.UWPSliderBaseUnit, Constants.UWPSliderBaseUnit, Constants.UWPSliderBaseUnit, 0);
        private static readonly Thickness rightButtonMargin = new Thickness(-24, 0, 0, 0);
        private static readonly Thickness leftButtonMargin = new Thickness(-8, 0, 0, 0);
        private static readonly double leftButtonBorderLeft = 20;
        private static readonly double rightButtonBorderLeft = 4;
        private CornerRadius SliderButtonCornerRadius
        {
            get { return (CornerRadius)GetValue(SliderButtonCornerRadiusProperty); }
            set { SetValue(SliderButtonCornerRadiusProperty, value); }
        }

        private static readonly DependencyProperty SliderButtonCornerRadiusProperty =
            DependencyProperty.Register(nameof(SliderButtonCornerRadius), typeof(CornerRadius), typeof(UWPSliderHorizontalBottom), new PropertyMetadata(null));

        private Thickness SliderButtonMargin
        {
            get { return (Thickness)GetValue(SliderButtonMarginProperty); }
            set { SetValue(SliderButtonMarginProperty, value); }
        }

        private static readonly DependencyProperty SliderButtonMarginProperty =
           DependencyProperty.Register(nameof(SliderButtonMargin), typeof(Thickness), typeof(UWPSliderHorizontalBottom), new PropertyMetadata(null));

        private double SliderButtonBorderLeft
        {
            get { return (double)GetValue(SliderButtonBorderLeftProperty); }
            set { SetValue(SliderButtonBorderLeftProperty, value); }
        }

        private static readonly DependencyProperty SliderButtonBorderLeftProperty =
              DependencyProperty.Register(nameof(SliderButtonBorderLeft), typeof(double), typeof(UWPSliderHorizontalBottom), new PropertyMetadata(null));

        static UWPSliderHorizontalBottom()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UWPSliderHorizontalBottom), new FrameworkPropertyMetadata(typeof(UWPSliderHorizontalBottom)));
        }

        protected override void CalculateVisibleLengths()
        {
            if (this.sliderCanvas == null) return;

            if (this.Position > this.sliderCanvas.ActualWidth / 2)
            {
                this.SliderButtonCornerRadius = rightCornerRadius;
                this.SliderButtonMargin = rightButtonMargin;
                this.SliderButtonBorderLeft = rightButtonBorderLeft;
                this.BarFillPosition = this.Position;
            }
            else
            {
                this.SliderButtonCornerRadius = leftCornerRadius;
                this.SliderButtonMargin = leftButtonMargin;
                this.SliderButtonBorderLeft = leftButtonBorderLeft;
                this.BarFillPosition = this.Position;
            }
        }
    }
}
