using Digimezzo.Foundation.WPF.Base;
using System;
using System.Windows;
using System.Windows.Media;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class UWPSliderHorizontal : Windows8SliderHorizontal
    {
        public double BarFillPosition
        {
            get { return Convert.ToDouble(BarFillPositionProperty); }
            set { SetValue(BarFillPositionProperty, value); }
        }

        public static readonly DependencyProperty BarFillPositionProperty = 
            DependencyProperty.Register(nameof(BarFillPosition), typeof(double), typeof(UWPSliderHorizontal), new PropertyMetadata(0.0));
      
        public Brush ButtonInnerBackground
        {
            get { return (Brush)GetValue(ButtonInnerBackgroundProperty); }
            set { SetValue(ButtonInnerBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ButtonInnerBackgroundProperty =
          DependencyProperty.Register(nameof(ButtonInnerBackground), typeof(Brush), typeof(UWPSliderHorizontal), new PropertyMetadata(null));

        static UWPSliderHorizontal()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UWPSliderHorizontal), new FrameworkPropertyMetadata(typeof(UWPSliderHorizontal)));
        }
     
        protected override void UpdatePosition()
        {
            base.UpdatePosition();
            this.CalculateVisibleLengths();
        }

        protected override void CalculatePosition()
        {
            base.CalculatePosition();
            this.CalculateVisibleLengths();
        }
       
        protected virtual void CalculateVisibleLengths()
        {
            if (this.sliderCanvas != null && this.sliderCanvas.ActualWidth != 0 && this.sliderButton != null)
            {
                this.BarFillPosition = this.Position * (this.sliderCanvas.ActualWidth - Constants.UWPSliderCanvasLengthOffset) /
                                       this.sliderCanvas.ActualWidth;
            }
        }
    }
}
