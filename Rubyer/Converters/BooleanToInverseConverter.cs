namespace Rubyer.Converters
{
    /// <summary>
    /// bool - 取反
    /// </summary>
    public class BooleanToInverseConverter : BooleanConverter<bool>
    {
        public BooleanToInverseConverter() 
            : base(false, true)
        {
        }
    }
}
