using Rubyer.Commons;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// AutoToolTipPlacement -> PlacementMode
    /// </summary>
    public class AutoToolTipPlacementToPlacementModeConverter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue|| values[1] == DependencyProperty.UnsetValue)
            {
                return DependencyProperty.UnsetValue;
            }

            var placement = ValidateArgument.NotNullOrEmptyCast<AutoToolTipPlacement>(values[0], "value[0]");
            var orientation = ValidateArgument.NotNullOrEmptyCast<Orientation>(values[1], "value[1]");
            return orientation switch
            {
                Orientation.Horizontal => placement switch
                {
                    AutoToolTipPlacement.None => PlacementMode.Top,
                    AutoToolTipPlacement.TopLeft => PlacementMode.Top,
                    AutoToolTipPlacement.BottomRight => PlacementMode.Bottom,
                },

                Orientation.Vertical => placement switch
                {
                    AutoToolTipPlacement.None => PlacementMode.Left,
                    AutoToolTipPlacement.TopLeft => PlacementMode.Left,
                    AutoToolTipPlacement.BottomRight => PlacementMode.Right,
                },
            };
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
