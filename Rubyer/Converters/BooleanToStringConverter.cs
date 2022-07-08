namespace Rubyer.Converters
{
    /// <summary>
    /// bool to string
    /// </summary>
    public class BooleanToStringConverter : BooleanConverter<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanToStringConverter"/> class.
        /// </summary>
        public BooleanToStringConverter()
            : base("true", "false")
        {
        }
    }
}
