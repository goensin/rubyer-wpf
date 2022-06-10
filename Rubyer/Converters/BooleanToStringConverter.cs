namespace Rubyer.Converters
{
    /// <summary>
    /// bool to string
    /// </summary>
    public class BooleanToStringConverter : BooleanConverter<string>
    {
        public BooleanToStringConverter()
            : base("true", "false")
        {
        }
    }
}
