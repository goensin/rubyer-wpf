using Rubyer;
using Rubyer.Commons;
using System;
using System.Globalization;
using System.Windows.Data;

namespace RubyerDemo.Converter
{
    /// <summary>
    /// 根据 IconType 获取 Icon
    /// </summary>
    public class GetIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var iconType = ValidateArgument.NotNullOrEmptyCast<IconType>(value, nameof(value));
            return new Icon { Type = iconType };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
