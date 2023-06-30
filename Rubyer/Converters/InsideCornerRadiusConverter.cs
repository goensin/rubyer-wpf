using Rubyer.Commons;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// 内圆角半径转换
    /// </summary>
    public class InsideCornerRadiusConverter : IMultiValueConverter
    {
        /// <summary>
        /// 内圆角半径类型
        /// </summary>
        public enum InsideCornerRadiusType
        {
            /// <summary>
            /// 全部方向
            /// </summary>
            All = 0,

            /// <summary>
            /// 上方向
            /// </summary>
            Top,

            /// <summary>
            /// 下方向
            /// </summary>
            Bottom,

            /// <summary>
            /// 左方向
            /// </summary>
            Left,

            /// <summary>
            /// 右方向
            /// </summary>
            Right,
        }

        /// <summary>
        /// 内圆角半径类型
        /// </summary>
        public InsideCornerRadiusType Type { get; set; }

        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
            {
                throw new ArgumentException("values length not less then 2.");
            }

            var thickness = ValidateArgument.NotNullOrEmptyCast<Thickness>(values[0], "values[0]");
            var cornerRadius = ValidateArgument.NotNullOrEmptyCast<CornerRadius>(values[1], "values[1]");
            var insideCornerRadius = cornerRadius.TopLeft - (thickness.Left / 2);
            if (insideCornerRadius < 0)
            {
                insideCornerRadius = 0;
            }

            return Type switch
            {
                InsideCornerRadiusType.All => new CornerRadius(insideCornerRadius),
                InsideCornerRadiusType.Top => new CornerRadius(insideCornerRadius, insideCornerRadius, 0, 0),
                InsideCornerRadiusType.Bottom => new CornerRadius(0, 0, insideCornerRadius, insideCornerRadius),
                InsideCornerRadiusType.Left => new CornerRadius(insideCornerRadius, 0, insideCornerRadius, 0),
                InsideCornerRadiusType.Right => new CornerRadius(0, insideCornerRadius, 0, insideCornerRadius),
                _ => new CornerRadius(insideCornerRadius),
            };
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
