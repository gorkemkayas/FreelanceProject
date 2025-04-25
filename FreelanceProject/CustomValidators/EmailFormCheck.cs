using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FreelanceProject.CustomValidators
{
    public class EmailFormCheck :ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var str = value!.ToString();
            var emailHead = str.Substring(0, str.IndexOf('@'));
            string firstChar = str.Substring(0,1);
            if(Regex.IsMatch(firstChar, @"[0-9]"))
            {
                return new ValidationResult("The first character of the email must not be a number.");
            }
            if(!Regex.IsMatch(emailHead, "^[a-z0-9]+$"))
                {
                return new ValidationResult($"Email address cannot contain special or capital characters.");
            }

            return ValidationResult.Success;
        }
    }
}
