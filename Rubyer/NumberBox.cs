using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Rubyer
{
    [TemplatePart(Name = TextBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = ButtonUpPartName, Type = typeof(ButtonBase))]
    [TemplatePart(Name = ButtonDownPartName, Type = typeof(ButtonBase))]
    public class NumberBox : Control
    {
        public const string TextBoxPartName = "PART_TextBox";
        public const string ButtonUpPartName = "PART_UpButton";
        public const string ButtonDownPartName = "PART_DownButton";

        private TextBox textBox;
        private ButtonBase upButton;
        private ButtonBase downButton;

        static NumberBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(typeof(NumberBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(TextBoxPartName) is TextBox textBox)
            {
                var textBinding = new Binding("Text");
                textBinding.Source = this;
                textBinding.Mode = BindingMode.TwoWay;
                textBox.SetBinding(TextBox.TextProperty, textBinding);
                this.textBox = textBox;
            }

            if (GetTemplateChild(ButtonUpPartName) is Button upButton)
            {
                upButton.Click += UpButton_Click;
                this.upButton = upButton;
            }

            if (GetTemplateChild(ButtonDownPartName) is Button downButton)
            {
                downButton.Click += DownButton_Click;
                this.downButton = downButton;
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #region propteries

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
           "Text", typeof(string), typeof(TimePicker), new PropertyMetadata(null, OnTextChanged));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion
    }
}
