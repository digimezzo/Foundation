using System;
using System.Windows.Media;

namespace Digimezzo.Foundation.Core.Helpers
{
    public class HSLColor
    {
        private double hue;
        private double saturation;
        private double luminosity;

        public int Hue
        {
            get
            {
                var t = (int)(this.hue * 60d);

                if (t > 360)
                {
                    t -= 360;
                }

                return t;
            }
            set
            {
                var t = value;

                if (t > 0)
                {
                    while (t > 360)
                    {
                        t -= 360;
                    }
                }

                else
                {
                    while (t < 0)
                    {
                        t += 360;
                    } 
                }

                this.hue = t / 60d;
            }
        }

        public int Saturation
        {
            get { return (int)(this.saturation * 100d); }
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentOutOfRangeException("Saturation", "Saturation only can be set between 0 and 100");
                }
                else
                {
                    this.saturation = value / 100d;
                }   
            }
        }

        public int Luminosity
        {
            get { return (int)(this.luminosity * 100d); }
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentOutOfRangeException("Luminosity", "Luminosity only can be set between 0 and 100");
                }
                else
                {
                    this.luminosity = value / 100d;
                }   
            }
        }

        public HSLColor(int H, int S, int L)
        {
            this.Hue = H;
            this.Saturation = S;
            this.Luminosity = L;
        }

        private HSLColor(double h, double s, double l)
        {
            this.hue = h;
            this.saturation = s;
            this.luminosity = l;
        }

        public Color ToRgb()
        {
            byte r, g, b;

            if (this.Saturation == 0)
            {
                r = (byte)Math.Round(this.luminosity * 255d);
                g = (byte)Math.Round(this.luminosity * 255d);
                b = (byte)Math.Round(this.luminosity * 255d);
            }
            else
            {
                double t1, t2;
                double th = this.hue / 6.0d;

                if (this.luminosity < 0.5d)
                {
                    t2 = this.luminosity * (1d + this.saturation);
                }
                else
                {
                    t2 = (this.luminosity + this.saturation) - (this.luminosity * this.saturation);
                }

                t1 = 2d * this.luminosity - t2;

                var tr = th + (1.0d / 3.0d);
                var tg = th;
                var tb = th - (1.0d / 3.0d);

                tr = ColorCalc(tr, t1, t2);
                tg = ColorCalc(tg, t1, t2);
                tb = ColorCalc(tb, t1, t2);

                r = (byte)Math.Round(tr * 255d);
                g = (byte)Math.Round(tg * 255d);
                b = (byte)Math.Round(tb * 255d);
            }

            return Color.FromRgb(r, g, b);
        }

        public HSLColor MoveNext(int step)
        {
            Hue += step;
            return this;
        }

        public static HSLColor GetFromRgb(Color color)
        {
            return GetFromRgb(color.R, color.G, color.B);
        }

        public static HSLColor GetFromRgb(byte R, byte G, byte B)
        {
            double r = R / 255d, g = G / 255d, b = B / 255d;
            double max = Math.Max(Math.Max(r, g), b), min = Math.Min(Math.Min(r, g), b);
            var delta = max - min;

            double h = 0, s = 0, l = (max + min) / 2d;

            if (delta != 0)
            {
                if (l < 0.5d)
                    s = delta / (max + min);
                else
                    s = delta / (2d - max - min);

                if (r == max)
                {
                    h = (g - b) / delta;
                }
                else if (g == max)
                {
                    h = 2d + (b - r) / delta;
                }
                else if (b == max)
                {
                    h = 4d + (r - g) / delta;
                }
            }

            return new HSLColor(h, s, l);
        }

        public static Color Normalize(Color color, int value)
        {
            if (value < 0 | value > 100)
            {
                return color;
            }

            HSLColor hslColor = HSLColor.GetFromRgb(color);

            if (hslColor.Luminosity < value)
            {
                hslColor.Luminosity = value;
            }
            else if (hslColor.Luminosity > 100 - value)
            {
                hslColor.Luminosity = 100 - value;
            }

            return hslColor.ToRgb();
        }

        private static double ColorCalc(double c, double t1, double t2)
        {
            if (c < 0)
            {
                c += 1d;
            }

            if (c > 1)
            {
                c -= 1d;
            }

            if (6.0d * c < 1.0d)
            {
                return t1 + (t2 - t1) * 6.0d * c;
            }

            if (2.0d * c < 1.0d)
            {
                return t2;
            }

            if (3.0d * c < 2.0d)
            {
                return t1 + (t2 - t1) * (2.0d / 3.0d - c) * 6.0d;
            }

            return t1;
        }
    }
}
