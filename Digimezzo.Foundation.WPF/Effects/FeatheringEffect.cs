using Digimezzo.Foundation.WPF.Utils;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Digimezzo.Foundation.WPF.Effects
{
    public class FeatheringEffect : ShaderEffect
    {
        private static PixelShader shader = new PixelShader()
        {
            UriSource = UriUtils.MakePackUri<FeatheringEffect>("Effects/FeatheringEffect.ps")
        };
  
        public Brush InputBackground
        {
            get => (Brush)GetValue(InputBackgroundProperty);
            set => SetValue(InputBackgroundProperty, value);
        }

        public double FeatheringRadius
        {
            get => (double) GetValue(FeatheringRadiusProperty);
            set => SetValue(FeatheringRadiusProperty, value);
        }

        public double TexWidth
        {
            get => (double)GetValue(TexWidthProperty);
            set => SetValue(TexWidthProperty, value);
        }
     
        public static DependencyProperty InputBackgroundProperty =
            RegisterPixelShaderSamplerProperty("InputBackground", typeof(FeatheringEffect), 0);

        public static DependencyProperty FeatheringRadiusProperty = DependencyProperty.Register("FeatheringRadius",
            typeof(double), typeof(FeatheringEffect), new UIPropertyMetadata(default(double), PixelShaderConstantCallback(0)));

        public static DependencyProperty TexWidthProperty = DependencyProperty.Register("TexWidth", typeof(double),
            typeof(FeatheringEffect), new UIPropertyMetadata(default(double), PixelShaderConstantCallback(1)));
      
        public FeatheringEffect()
        {
            PixelShader = shader;
            UpdateShaderValue(InputBackgroundProperty);
            UpdateShaderValue(FeatheringRadiusProperty);
            UpdateShaderValue(TexWidthProperty);
        }
    }
}