using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace ENBOrganizer.App.Validation
{
    public class FilePathValidationRule : NotEmptyValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult notEmptyValidationResult = base.Validate(value, cultureInfo);

            if (!notEmptyValidationResult.Equals(ValidationResult.ValidResult))
                return notEmptyValidationResult;

            return File.Exists(value.ToString()) ? ValidationResult.ValidResult : new ValidationResult(false, "File does not exist.");
        }
    }
}