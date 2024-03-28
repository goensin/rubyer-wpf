using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rubyer
{
    /// <summary>
    /// IP 地址输入控件
    /// </summary>
    [TemplatePart(Name = Octet1BoxName, Type = typeof(TextBox))]
    [TemplatePart(Name = Octet2BoxName, Type = typeof(TextBox))]
    [TemplatePart(Name = Octet3BoxName, Type = typeof(TextBox))]
    [TemplatePart(Name = Octet4BoxName, Type = typeof(TextBox))]
    public class IpAddressControl : Control
    {
        const string Octet1BoxName = "PART_Octet1Box";
        const string Octet2BoxName = "PART_Octet2Box";
        const string Octet3BoxName = "PART_Octet3Box";
        const string Octet4BoxName = "PART_Octet4Box";

        private TextBox numericBox1;
        private TextBox numericBox2;
        private TextBox numericBox3;
        private TextBox numericBox4;
        private List<TextBox> allTextBoxs;

        /// <summary>
        /// 1#字节
        /// </summary>
        public static readonly DependencyProperty Octet1Property =
            DependencyProperty.Register("Octet1", typeof(string), typeof(IpAddressControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnOctetChanged, OnCoerceOctet));

        /// <summary>
        /// 2#字节
        /// </summary>
        public static readonly DependencyProperty Octet2Property =
            DependencyProperty.Register("Octet2", typeof(string), typeof(IpAddressControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnOctetChanged, OnCoerceOctet));

        /// <summary>
        /// 3#字节
        /// </summary>
        public static readonly DependencyProperty Octet3Property =
            DependencyProperty.Register("Octet3", typeof(string), typeof(IpAddressControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnOctetChanged, OnCoerceOctet));

        /// <summary>
        /// 4#字节
        /// </summary>
        public static readonly DependencyProperty Octet4Property =
            DependencyProperty.Register("Octet4", typeof(string), typeof(IpAddressControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnOctetChanged, OnCoerceOctet));

        /// <summary>
        /// IP 地址
        /// </summary>
        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register("Address", typeof(string), typeof(IpAddressControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnAddressChanged));

        /// <summary>
        /// 1#字节
        /// </summary>
        public string Octet1
        {
            get { return (string)GetValue(Octet1Property); }
            set { SetValue(Octet1Property, value); }
        }

        /// <summary>
        /// 2#字节
        /// </summary>
        public string Octet2
        {
            get { return (string)GetValue(Octet2Property); }
            set { SetValue(Octet2Property, value); }
        }

        /// <summary>
        /// 3#字节
        /// </summary>
        public string Octet3
        {
            get { return (string)GetValue(Octet3Property); }
            set { SetValue(Octet3Property, value); }
        }

        /// <summary>
        /// 4#字节
        /// </summary>
        public string Octet4
        {
            get { return (string)GetValue(Octet4Property); }
            set { SetValue(Octet4Property, value); }
        }

        /// <summary>
        /// IP 地址
        /// </summary>
        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            numericBox1 = GetTemplateChild(Octet1BoxName) as TextBox;
            numericBox2 = GetTemplateChild(Octet2BoxName) as TextBox;
            numericBox3 = GetTemplateChild(Octet3BoxName) as TextBox;
            numericBox4 = GetTemplateChild(Octet4BoxName) as TextBox;
            allTextBoxs = [numericBox1, numericBox2, numericBox3, numericBox4];

            foreach (var textBox in allTextBoxs)
            {
                WeakEventManager<UIElement, TextCompositionEventArgs>.AddHandler(textBox, "PreviewTextInput", TextBox_PreviewTextInput);
                WeakEventManager<UIElement, KeyEventArgs>.AddHandler(textBox, "PreviewKeyDown", TextBox_PreviewKeyDown);
                textBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnPasteIpAddress, null));
                textBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, OnCopyIpAddress, null));
            }
        }

        private static object OnCoerceOctet(DependencyObject d, object baseValue)
        {
            if (baseValue is { } && int.TryParse(baseValue.ToString(), out int octet))
            {
                if (octet < 0)
                {
                    octet = 0;
                }

                if (octet > 255)
                {
                    octet = 255;
                }

                return octet.ToString();
            }

            return baseValue;
        }

        private static void FocusOtherTextBox(IpAddressControl ip, int num, bool isSelectAll = true, bool isSelectionStart = true)
        {
            if (ip.IsLoaded && num >= 0 && num < ip.allTextBoxs.Count)
            {
                var textBox = ip.allTextBoxs[num];
                textBox.Focus();
                if (isSelectAll)
                {
                    textBox.SelectAll();
                }
                else
                {
                    textBox.SelectionStart = isSelectionStart ? 0 : textBox.Text.Length;
                }
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[0-9]");
            e.Handled = !regex.IsMatch(e.Text);

            var textBox = (TextBox)sender;
            var index = allTextBoxs.IndexOf(textBox);
            if ((e.Text == "." || (!e.Handled && textBox.Text.Length >= 3)) && textBox.SelectedText.Length == 0)
            {
                FocusOtherTextBox(this, ++index);
                e.Handled = true;
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (e.Key == Key.Right && textBox.SelectedText.Length == 0) // ->
            {
                if (textBox.SelectionStart == textBox.Text.Length)
                {
                    var index = allTextBoxs.IndexOf(textBox);
                    FocusOtherTextBox(this, ++index, isSelectAll: false);
                    e.Handled = true;
                }
            }

            if (e.Key == Key.Space && textBox.SelectedText.Length == 0) // space
            {
                var index = allTextBoxs.IndexOf(textBox);
                FocusOtherTextBox(this, ++index);
                e.Handled = true;
            }

            if (e.Key == Key.Left && textBox.SelectedText.Length == 0) // <-
            {
                if (textBox.SelectionStart == 0)
                {
                    var index = allTextBoxs.IndexOf(textBox);
                    FocusOtherTextBox(this, --index, isSelectAll: false, isSelectionStart: false);
                    e.Handled = true;
                }
            }

            if (e.Key == Key.Back && string.IsNullOrEmpty(textBox.Text)) // back
            {
                var index = allTextBoxs.IndexOf(textBox);
                FocusOtherTextBox(this, --index);
                e.Handled = true;
            }
        }

        private static string GetNumber(string octet) => string.IsNullOrEmpty(octet) ? "0" : octet;

        private static void OnOctetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ip = (IpAddressControl)d;
            ip.Address = $"{GetNumber(ip.Octet1)}.{GetNumber(ip.Octet2)}.{GetNumber(ip.Octet3)}.{GetNumber(ip.Octet4)}";

            // 当长度为 3 时，聚焦到下一个
            if (e.NewValue.ToString().Length >= 3 && int.TryParse(e.Property.Name.Replace("Octet", ""), out int num))
            {
                FocusOtherTextBox(ip, num);
            }
        }

        private static void OnAddressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ip = (IpAddressControl)d;
            var octets = ip.Address.Split('.');
            if (octets.Length >= 4)
            {
                ip.Octet1 = octets[0];
                ip.Octet2 = octets[1];
                ip.Octet3 = octets[2];
                ip.Octet4 = octets[3];
            }
        }

        private void OnCopyIpAddress(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetText(Address);
        }

        private void OnPasteIpAddress(object sender, ExecutedRoutedEventArgs e)
        {
            var text = Clipboard.GetText();
            string pattern = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
            if (Regex.IsMatch(text, pattern))
            {
                Address = text;
            }
        }
    }
}
