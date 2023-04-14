using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// double => Thickness
    /// </summary>
    public class DoubleToThicknessConverter : IValueConverter
    {
        /// <summary>
        /// Thickness 模式
        /// </summary>
        public ThicknessMode Mode { get; set; }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double thickness)
            {
                switch (Mode)
                {
                    case ThicknessMode.All:
                    default:
                        return new Thickness(thickness);

                    case ThicknessMode.LeftAndRight:
                        return new Thickness(thickness, 0, thickness, 0);

                    case ThicknessMode.Left:
                        return new Thickness(thickness, 0, 0, 0);

                    case ThicknessMode.Right:
                        return new Thickness(0, 0, thickness, 0);

                    case ThicknessMode.TopAndBottom:
                        return new Thickness(0, thickness, 0, thickness);

                    case ThicknessMode.Top:
                        return new Thickness(0, thickness, 0, 0);

                    case ThicknessMode.Bottom:
                        return new Thickness(0, 0, 0, thickness);
                }
            }

            return new Thickness(0);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Thickness 模式
    /// </summary>
    public enum ThicknessMode
    {
        All = 0,
        LeftAndRight,
        Left,
        Right,
        TopAndBottom,
        Top,
        Bottom,
    }
}
