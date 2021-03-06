﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class SplitView : Control, IDisposable
    {
        private ContentPresenter pane;
        private ContentPresenter content;
        private Border overlay;
        private Button button;

        public Brush ButtonHoveredBackground
        {
            get { return (Brush)GetValue(ButtonHoveredBackgroundProperty); }
            set { SetValue(ButtonHoveredBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ButtonHoveredBackgroundProperty =
           DependencyProperty.Register(nameof(ButtonHoveredBackground), typeof(Brush), typeof(SplitView), new PropertyMetadata(null));

        public Brush ButtonPressedBackground
        {
            get { return (Brush)GetValue(ButtonPressedBackgroundProperty); }
            set { SetValue(ButtonPressedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ButtonPressedBackgroundProperty =
           DependencyProperty.Register(nameof(ButtonPressedBackground), typeof(Brush), typeof(SplitView), new PropertyMetadata(null));

        public bool ShowButton
        {
            get { return (bool)GetValue(ShowButtonProperty); }
            set { SetValue(ShowButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowButtonProperty =
           DependencyProperty.Register(nameof(ShowButton), typeof(bool), typeof(SplitView), new PropertyMetadata(true));

        public double ButtonWidth
        {
            get { return (double)GetValue(ButtonWidthProperty); }
            set { SetValue(ButtonWidthProperty, value); }
        }

        public static readonly DependencyProperty ButtonWidthProperty =
           DependencyProperty.Register(nameof(ButtonWidth), typeof(double), typeof(SplitView), new PropertyMetadata(48.0));

        public double ButtonHeight
        {
            get { return (double)GetValue(ButtonHeightProperty); }
            set { SetValue(ButtonHeightProperty, value); }
        }

        public static readonly DependencyProperty ButtonHeightProperty =
           DependencyProperty.Register(nameof(ButtonHeight), typeof(double), typeof(SplitView), new PropertyMetadata(48.0));

        public object ButtonContent
        {
            get { return (object)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }

        public static readonly DependencyProperty ButtonContentProperty =
           DependencyProperty.Register(nameof(ButtonContent), typeof(object), typeof(SplitView), new PropertyMetadata(null));

        public Brush OverlayBackground
        {
            get { return (Brush)GetValue(OverlayBackgroundProperty); }
            set { SetValue(OverlayBackgroundProperty, value); }
        }

        public static readonly DependencyProperty OverlayBackgroundProperty =
           DependencyProperty.Register(nameof(OverlayBackground), typeof(Brush), typeof(SplitView), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));

        public object Pane
        {
            get { return (object)GetValue(PaneProperty); }
            set { SetValue(PaneProperty, value); }
        }

        public static readonly DependencyProperty PaneProperty =
           DependencyProperty.Register(nameof(Pane), typeof(object), typeof(SplitView), new PropertyMetadata(null));

        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
          DependencyProperty.Register(nameof(Content), typeof(object), typeof(SplitView), new PropertyMetadata(null));

        public bool IsPaneOpen
        {
            get { return (bool)GetValue(IsPaneOpenProperty); }
            set { SetValue(IsPaneOpenProperty, value); }
        }

        public static readonly DependencyProperty IsPaneOpenProperty =
            DependencyProperty.Register(nameof(IsPaneOpen), typeof(bool), typeof(SplitView), new PropertyMetadata(false, OnIsPaneOpenChanged));


        public double OpenPaneLength
        {
            get { return (double)GetValue(OpenPaneLengthProperty); }
            set { SetValue(OpenPaneLengthProperty, value); }
        }

        public static readonly DependencyProperty OpenPaneLengthProperty =
          DependencyProperty.Register(nameof(OpenPaneLength), typeof(double), typeof(SplitView), new PropertyMetadata(200.0));

        public event EventHandler PaneOpened = delegate { };
        public event EventHandler PaneClosed = delegate { };

        private static void OnIsPaneOpenChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SplitView splitView = o as SplitView;
            bool isPaneOpen = (bool)e.NewValue;

            if (isPaneOpen)
            {
                splitView.OpenPane();
            }
            else
            {
                splitView.ClosePane();
            }
        }

        static SplitView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitView), new FrameworkPropertyMetadata(typeof(SplitView)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.pane = (ContentPresenter)GetTemplateChild("PART_Pane");
            this.content = (ContentPresenter)GetTemplateChild("PART_Content");
            this.overlay = (Border)GetTemplateChild("PART_Overlay");
            this.button = (Button)GetTemplateChild("PART_Button");

            if (this.pane != null)
            {
                this.pane.Margin = new Thickness(-this.OpenPaneLength - 1, 0, 0, 0);
            }

            if (this.overlay != null)
            {
                this.overlay.MouseUp += MouseUpHandler;
            }

            if (this.button != null)
            {
                this.button.Click += Button_Click;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.IsPaneOpen = !this.IsPaneOpen;
        }

        private void MouseUpHandler(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.IsPaneOpen = false;
        }

        private void OpenPane()
        {
            if (this.pane != null)
            {
                var marginAnimation = new ThicknessAnimation
                {
                    From = new Thickness(-this.OpenPaneLength - 1, 0, 0, 0),
                    To = new Thickness(0, 0, 0, 0),
                    Duration = TimeSpan.FromMilliseconds(150)
                };

                this.pane.BeginAnimation(ContentPresenter.MarginProperty, marginAnimation);
            }

            this.PaneOpened(this, new EventArgs());

            this.ShowOverlayAnimation();
        }
        private void ClosePane()
        {
            if (this.pane != null)
            {
                var marginAnimation = new ThicknessAnimation
                {
                    From = new Thickness(0, 0, 0, 0),
                    To = new Thickness(-this.OpenPaneLength - 1, 0, 0, 0),
                    Duration = TimeSpan.FromMilliseconds(150)
                };

                this.pane.BeginAnimation(ContentPresenter.MarginProperty, marginAnimation);
            }

            this.PaneClosed(this, new EventArgs());

            this.HideOverlayAnimationAsync();
        }

        private void ShowOverlayAnimation()
        {
            if (this.overlay == null)
            {
                return;
            }

            this.overlay.Visibility = Visibility.Visible;
            this.AnimateOverlayOpacity(0.0, 1.0);
        }

        private async void HideOverlayAnimationAsync()
        {
            if (this.overlay == null)
            {
                return;
            }

            this.AnimateOverlayOpacity(1.0, 0.0);
            await Task.Delay(250); // Give the storyboard time to finish before collapsing the overlay
            this.overlay.Visibility = Visibility.Collapsed;
        }

        private void AnimateOverlayOpacity(double startOpacity, double endOpacity)
        {
            if (this.overlay == null)
            {
                return;
            }

            DoubleAnimation opacityAnimation = new DoubleAnimation() { From = startOpacity, To = endOpacity, Duration = TimeSpan.FromMilliseconds(150) };
            var sb = new Storyboard();
            sb.Children.Add(opacityAnimation);
            Storyboard.SetTargetName(this.overlay, "PART_Overlay");
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UserControl.OpacityProperty));
            sb.Begin(this.overlay);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.button != null)
                    {
                        this.button.Click -= Button_Click;
                    }

                    if (this.overlay != null)
                    {
                        this.overlay.MouseUp -= MouseUpHandler;
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
