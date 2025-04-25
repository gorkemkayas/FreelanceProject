using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FreelanceProject.CustomValidators
{
    public class StringCheck :ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var str = value.ToString();
            if(string.IsNullOrWhiteSpace(str))
            {
                return ValidationResult.Success;
            }
            if (string.IsNullOrEmpty(str))
            {
                return new ValidationResult("This field is required.");
            }
            if (str.Length < 3)
            {
                return new ValidationResult("The string must be at least 5 characters long.");
            }
            if (str.Length > 20)
            {
                return new ValidationResult("The string must be at most 20 characters long.");
            }
            if (Regex.IsMatch(str, @"\d"))
            {
                return new ValidationResult("The string must not contain numbers.");
            }
            if (!Regex.IsMatch(str, "^[a-zA-ZçğıöşüÇĞİÖŞÜ]+$"))
            {
                return new ValidationResult($"' {validationContext.DisplayName}' must consist of alphabetical characters only.");
            }

            return ValidationResult.Success;
        }
    }
}
