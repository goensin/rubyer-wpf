using System.Globalization;
using System.Windows.Controls;

namespace RubyerDemo.ValidationRules
{
    /// <summary>
    /// 不为空验证
    /// </summary>
    public class RequiredRule : ValidationRule
    {
        /// <inheritdoc/>
        public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
        {
            if (value is null)
            {
                return new ValidationResult(false, $"值不能为空");
            }

            return ValidationResult.ValidResult;
        }
    }
}
