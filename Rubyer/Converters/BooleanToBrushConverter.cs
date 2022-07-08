using System.Windows.Media;

namespace Rubyer.Converters
{
    /// <summary>
    /// bool to brush
    /// </summary>
    public class BooleanToBrushConverter : BooleanConverter<Brush>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanToBrushConverter"/> class.
        /// </summary>
        public BooleanToBrushConverter()
            : base(Brushes.Green, Brushes.Red)
        {
        }
    }
}
