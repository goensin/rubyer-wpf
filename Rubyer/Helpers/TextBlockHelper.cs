using System;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    public class TextBlockHelper
    {
        /// <summary>
        /// 当被 Trim 时显示 Tool Tip
        /// </summary>
        public static readonly DependencyProperty IsTrimmedShowToolTipProperty =
            DependencyProperty.RegisterAttached("IsTrimmedShowToolTip", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, OnIsTrimmedShowToolTipChanged));

        private static void OnIsTrimmedShowToolTipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock textBlock)
            {
                if (GetIsTrimmedShowToolTip(textBlock))
                {
                    GetIsTrimming(textBlock);
                }
            }
        }

        private static void TextBlock_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
           
        }

        private static void TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var textBlock = (TextBlock)sender;
        }

        public static bool GetIsTrimming(TextBlock textBlock)
        {

            if (textBlock.TextTrimming == TextTrimming.None)
            {
                return false;
            }


            if (textBlock.TextWrapping == TextWrapping.NoWrap)  //比较长度
            {
                Size size = new Size(double.MaxValue, textBlock.RenderSize.Height);
                textBlock.Measure(size);

                if (textBlock.DesiredSize.Width > textBlock.RenderSize.Width)
                {
                    textBlock.Measure(textBlock.RenderSize);
                    return true;
                }

            }
            else  //比较高度
            {
                Size size = new Size(textBlock.RenderSize.Width,double.MaxValue);
                textBlock.Measure(size);
                if (textBlock.DesiredSize.Height > textBlock.RenderSize.Height)
                {
                    textBlock.Measure(textBlock.RenderSize);
                    return true;
                }
            }
            return false;
        }

        public static bool GetIsTrimmedShowToolTip(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsTrimmedShowToolTipProperty);
        }

        public static void SetIsTrimmedShowToolTip(DependencyObject obj, bool value)
        {
            obj.SetValue(IsTrimmedShowToolTipProperty, value);
        }
    }
}
