using Digimezzo.Foundation.WPF.Base;
using System;
using System.Windows;
using System.Windows.Media;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class UWPSliderVertical : Windows8SliderVertical
    {
        public double BarFillPosition
        {
            get { return Convert.ToDouble(BarFillPositionProperty); }
            set { SetValue(BarFillPositionProperty, value); }
        }
        public Brush ButtonInnerBackground
        {
            get { return (Brush)GetValue(ButtonInnerBackgroundProperty); }
            set { SetValue(ButtonInnerBackgroundProperty, value); }
        }
      
        public static readonly DependencyProperty BarFillPositionProperty = DependencyProperty.Register("BarFillPosition", typeof(double), typeof(UWPSliderVertical), new PropertyMetadata(0.0));
        public static readonly DependencyProperty ButtonInnerBackgroundProperty = DependencyProperty.Register("ButtonInnerBackground", typeof(Brush), typeof(UWPSliderVertical), new PropertyMetadata(null));
      
        static UWPSliderVertical()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UWPSliderVertical), new FrameworkPropertyMetadata(typeof(UWPSliderVertical)));
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
    
        private void CalculateVisibleLengths()
        {
            if (this.sliderCanvas != null && this.sliderCanvas.ActualWidth != 0 && this.sliderButton != null)
            {
                this.BarFillPosition = this.Position * (this.sliderCanvas.ActualHeight - Constants.UWPSliderCanvasLengthOffset) /
                                       this.sliderCanvas.ActualHeight;
            }
        }
    }
}
