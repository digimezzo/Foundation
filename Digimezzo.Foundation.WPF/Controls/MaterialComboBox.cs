﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Digimezzo.Foundation.WPF.Controls
{
    public class MaterialComboBox : ComboBox, IDisposable
    {
        private TextBox editableTextBox;
        private TextBlock errorLabel;
        private TextBlock inputLabel;
        private bool previousIsFloating;
        private Grid panel;
        private Border inputLine;
        private Border inputLineUnfocused;
        private ToggleButton toggleButton;
        private double opacity = 0.55;
        private bool isFocused;
        private Border dropDownBorder;
        private bool isValueChanged;

        public TextBlock InputLabel => this.inputLabel;

        public Brush ErrorForeground
        {
            get { return (Brush)GetValue(ErrorForegroundProperty); }
            set { SetValue(ErrorForegroundProperty, value); }
        }

        public static readonly DependencyProperty ErrorForegroundProperty =
            DependencyProperty.Register(nameof(ErrorForeground), typeof(Brush), typeof(MaterialComboBox), new PropertyMetadata(Brushes.Red));

        public string ErrorText
        {
            get { return (string)GetValue(ErrorTextProperty); }
            set { SetValue(ErrorTextProperty, value); }
        }

        public static readonly DependencyProperty ErrorTextProperty =
            DependencyProperty.Register(nameof(ErrorText), typeof(string), typeof(MaterialComboBox), new PropertyMetadata("Invalid"));

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        public static readonly DependencyProperty IsValidProperty =
           DependencyProperty.Register(nameof(IsValid), typeof(bool), typeof(MaterialComboBox), new PropertyMetadata(true));

        public ValidationMode ValidationMode
        {
            get { return (ValidationMode)GetValue(ValidationModeProperty); }
            set { SetValue(ValidationModeProperty, value); }
        }

        public static readonly DependencyProperty ValidationModeProperty =
            DependencyProperty.Register(nameof(ValidationMode), typeof(ValidationMode), typeof(MaterialComboBox), new PropertyMetadata(ValidationMode.None));

        public bool IsFloating
        {
            get { return (bool)GetValue(IsFloatingProperty); }
            set { SetValue(IsFloatingProperty, value); }
        }

        public static readonly DependencyProperty IsFloatingProperty =
            DependencyProperty.Register(nameof(IsFloating), typeof(bool), typeof(MaterialComboBox), new PropertyMetadata(false));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(MaterialComboBox), new PropertyMetadata(null, new PropertyChangedCallback(OnLabelPropertyChanged)));

        private static void OnLabelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MaterialComboBox box = d as MaterialComboBox;

            if (box != null && box.inputLabel != null)
            {
                box.inputLabel.Text = box.Label;
            }
        }

        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register(nameof(Accent), typeof(Brush), typeof(MaterialComboBox), new PropertyMetadata(Brushes.Red));

        static MaterialComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialComboBox), new FrameworkPropertyMetadata(typeof(MaterialComboBox)));
            ForegroundProperty.OverrideMetadata(typeof(MaterialComboBox), new FrameworkPropertyMetadata(null, OnForegroundChanged));
        }

        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is MaterialComboBox))
            {
                return;

            }

            var box = (MaterialComboBox)d;
            box.SetCursorColor();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.inputLabel = (TextBlock)GetTemplateChild("PART_InputLabel");
            this.toggleButton = (ToggleButton)GetTemplateChild("ToggleButton");
            this.inputLine = (Border)GetTemplateChild("PART_InputLine");
            this.inputLineUnfocused = (Border)GetTemplateChild("PART_InputLineUnfocused");
            this.dropDownBorder = (Border)GetTemplateChild("DropDownBorder");
            this.panel = (Grid)GetTemplateChild("PART_Panel");
            this.editableTextBox = (TextBox)GetTemplateChild("PART_EditableTextBox");
            this.errorLabel = (TextBlock)GetTemplateChild("PART_ErrorLabel");
            this.toggleButton.Opacity = this.opacity;
            this.inputLineUnfocused.Opacity = this.opacity;
            this.inputLabel.Text = this.Label;
            this.inputLabel.Opacity = this.opacity;
            this.inputLabel.MouseDown += InputLabel_MouseDown;
            this.panel.Margin = this.IsFloating ? new Thickness(0, this.GetSmallFontSize() + this.GetMargin(), 0, 0) : new Thickness(0);
            this.dropDownBorder.Background = this.Background == null ? Brushes.White : this.Background;

            this.editableTextBox.TextChanged += this.EditableTextBox_TextChanged;

            // Initial state of the input label
            this.SetInputLabel();

            // Initial state of the error label
            this.SetErrorLabel();

            // Initial validation
            this.Validate();
        }

        private void SetErrorLabel()
        {
            if(this.errorLabel == null)
            {
                return;
            }

            if (this.ValidationMode.Equals(ValidationMode.Text) || this.ValidationMode.Equals(ValidationMode.Number))
            {
                this.errorLabel.Visibility = Visibility.Visible;
                this.errorLabel.FontSize = this.GetSmallFontSize();
                this.errorLabel.Margin = new Thickness(0, this.GetMargin(), 0, 0);
            }
            else
            {
                this.errorLabel.Visibility = Visibility.Collapsed;
                this.errorLabel.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        private void EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Validate();
        }

        private void SetCursorColor()
        {
            if (!(this.Foreground is SolidColorBrush))
            {
                return;
            }

            if (this.editableTextBox == null)
            {
                return;
            }

            // This workaround changes the color of the cursor
            // WPF sets it to the inverse of the Background color  
            var col = ((SolidColorBrush)this.Foreground).Color;
            this.editableTextBox.Background = new SolidColorBrush(Color.FromRgb((byte)~col.R, (byte)~col.G, (byte)~col.B));
        }

        private void InputLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Workaround: InputLabel seems to block mouse click which prevents the drop down to open
            this.IsDropDownOpen = !this.IsDropDownOpen;
        }

        private double GetSmallFontSize()
        {
            return this.FontSize > 14 ? this.FontSize * 0.7 : 10;
        }

        private double GetMargin()
        {
            return this.FontSize * 0.3;
        }

        private void SetInputLabelForeground(bool mustFocus)
        {
            if (this.inputLabel == null)
            {
                return;
            }

            this.inputLabel.Foreground = mustFocus ? this.Accent : this.Foreground;
            this.inputLabel.Opacity = mustFocus ? 1.0 : this.opacity;
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            this.isValueChanged = true;
            this.SetInputLabel();
            this.Validate();
        }

        private void SetInputLabel()
        {
            if (this.IsFloating)
            {
                this.AnimateInputLabel(!string.IsNullOrEmpty(this.Text));
            }
            else
            {
                this.SetInputLabelText(!string.IsNullOrEmpty(this.Text));
            }
        }

        private void SetInputLabelText(bool mustClear)
        {
            if (this.inputLabel == null)
            {
                return;
            }

            this.inputLabel.Text = mustClear ? string.Empty : this.Label;
        }

        private void AnimateInputLabel(bool mustFloat)
        {
            if (this.inputLabel == null)
            {
                return;
            }

            var duration = new TimeSpan(0, 0, 0, 0, 200);

            this.SetInputLabelForeground(mustFloat);

            double smallFontSize = 0;
            double margin = 2;

            if (this.FontSize != double.NaN)
            {
                smallFontSize = this.GetSmallFontSize();
                margin = this.GetMargin();
            }

            double offset = smallFontSize + margin;

            var enlarge = new DoubleAnimation(smallFontSize, this.FontSize, duration);
            var reduce = new DoubleAnimation(this.FontSize, smallFontSize, duration);

            var moveUp = new ThicknessAnimation(new Thickness(2, 0, 2, 0), new Thickness(2, -offset, 2, offset), duration);
            var moveDown = new ThicknessAnimation(new Thickness(2, -offset, 2, offset), new Thickness(2, 0, 2, 0), duration);

            if (!previousIsFloating.Equals(mustFloat))
            {
                previousIsFloating = mustFloat;
                this.inputLabel.BeginAnimation(FontSizeProperty, mustFloat ? reduce : enlarge);
                this.inputLabel.BeginAnimation(MarginProperty, mustFloat ? moveUp : moveDown);
            }
        }

        protected override void OnDropDownOpened(EventArgs e)
        {
            base.OnDropDownOpened(e);
            this.AnimateInputLine(true);
            this.SetInputLabelForeground(true);
            this.AnimateInputLabel(true);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (!this.IsDropDownOpen)
            {
                this.AnimateInputLine(false);
                this.SetInputLabelForeground(false);
            }
        }

        private void AnimateInputLine(bool mustFocus)
        {
            if (this.inputLine == null)
            {
                return;
            }

            this.isFocused = mustFocus;

            var duration = new TimeSpan(0, 0, 0, 0, 200);
            var enlarge = new DoubleAnimation(0, this.ActualWidth, duration);
            var reduce = new DoubleAnimation(this.ActualWidth, 0, duration);

            this.inputLine.BeginAnimation(WidthProperty, mustFocus ? enlarge : reduce);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            this.ResizeInputLine();
        }

        private void ResizeInputLine()
        {
            if (this.inputLine == null)
            {
                return;
            }

            if (this.isFocused)
            {
                // Set width animation to null to be able to set the width manually
                this.inputLine.BeginAnimation(WidthProperty, null);
                this.inputLine.Width = this.ActualWidth;
            }
        }

        private void Validate()
        {
            switch (this.ValidationMode)
            {
                case ValidationMode.Number:
                    this.ValidateNumber();
                    break;
                case ValidationMode.Text:
                    this.ValidateText();
                    break;
                case ValidationMode.Date:
                case ValidationMode.None:
                default:
                    break;
            }
        }

        private void ValidateText()
        {
            if(this.errorLabel == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(this.Text))
            {
                // Checking isValueChanged prevents showing an error when the control initially loads with a empty value
                this.errorLabel.Text = this.isValueChanged ? this.ErrorText : String.Empty;
                this.IsValid = false;
            }
            else
            {
                this.errorLabel.Text = String.Empty;
                this.IsValid = true;
            }
        }

        private void ValidateNumber()
        {
            if (this.errorLabel == null)
            {
                return;
            }

            bool isNumberValid = false;

            if (string.IsNullOrEmpty(this.Text))
            {
                isNumberValid = false;
            }
            else
            {
                double number = 0;
                isNumberValid = double.TryParse(this.Text, out number);
            }

            if (isNumberValid)
            {
                this.errorLabel.Text = String.Empty;
                this.IsValid = true;
            }
            else
            {
                // Checking isValueChanged prevents showing an error when the control initially loads with a empty value
                this.errorLabel.Text = this.isValueChanged ? this.ErrorText : String.Empty;
                this.IsValid = false;
            }
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.editableTextBox != null)
                    {
                        this.editableTextBox.TextChanged -= this.EditableTextBox_TextChanged;
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
