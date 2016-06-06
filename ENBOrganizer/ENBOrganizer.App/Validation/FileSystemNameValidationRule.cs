using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace ENBOrganizer.App.Validation
{
    public class FileSystemNameValidationRule : NotEmptyValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult notEmptyValidationResult = base.Validate(value, cultureInfo);

            if (!notEmptyValidationResult.Equals(ValidationResult.ValidResult))
                return notEmptyValidationResult;

            char[] path = value.ToString().ToCharArray();

            foreach (char c in Path.GetInvalidFileNameChars())
                if (path.Contains(c))
                    return new ValidationResult(false, "Field contains invalid characters.");

            return ValidationResult.ValidResult;
        }
    }
}
