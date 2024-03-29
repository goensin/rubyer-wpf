﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// The get width converter.
    /// </summary>
    public class GetWidthConverter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)values[0] - (double)values[1] * 2;
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}