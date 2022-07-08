namespace Rubyer.Converters
{
    /// <summary>
    /// bool - 取反
    /// </summary>
    public class BooleanToInverseConverter : BooleanConverter<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanToInverseConverter"/> class.
        /// </summary>
        public BooleanToInverseConverter() 
            : base(false, true)
        {
        }
    }
}
