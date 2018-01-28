using Digimezzo.Foundation.WPF.Base;
using System.Windows;
using System.Windows.Input;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class Windows8SliderVertical : SliderBase
    {

        static Windows8SliderVertical()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Windows8SliderVertical), new FrameworkPropertyMetadata(typeof(Windows8SliderVertical)));
        }

        protected override void UpdatePosition()
        {
            if (this.sliderCanvas == null) return;

            this.Position = this.sliderCanvas.ActualHeight - Mouse.GetPosition(this.sliderCanvas).Y;

            if (this.Position > this.sliderCanvas.ActualHeight)
            {
                this.Position = this.sliderCanvas.ActualHeight;
            }

            if (this.Position < 0.0)
            {
                this.Position = 0.0;
            }

        }

        protected override void CalculatePosition()
        {
            if (this.sliderCanvas == null) return;

            if (!this.isCalculating)
            {
                this.isCalculating = true;

                if ((this.Maximum - this.Minimum) > 0 && this.sliderCanvas.ActualHeight > 0)
                {
                    this.Position = ((this.Value - this.Minimum) / (this.Maximum - this.Minimum)) * this.sliderCanvas.ActualHeight;
                }
                else
                {
                    this.Position = 0;
                }

                this.isCalculating = false;
            }
        }

        protected override void CalculateValue()
        {
            if (this.sliderCanvas == null) return;

            if (!this.isCalculating)
            {
                this.isCalculating = true;

                if (this.sliderCanvas.ActualHeight > 0)
                {
                    this.Value = ((this.Position * (this.Maximum - this.Minimum)) / this.sliderCanvas.ActualHeight) + this.Minimum;
                }
                else
                {
                    this.Value = 0;
                }

                this.isCalculating = false;
            }
        }
    }
}
