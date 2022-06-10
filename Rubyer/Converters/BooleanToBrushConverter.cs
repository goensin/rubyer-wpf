using System.Windows.Media;

namespace Rubyer.Converters
{
    /// <summary>
    /// bool to brush
    /// </summary>
    public class BooleanToBrushConverter : BooleanConverter<Brush>
    {
        public BooleanToBrushConverter()
            : base(Brushes.Green, Brushes.Red)
        {
        }
    }
}
