using eDoctor.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace eDoctor.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class LoginNameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        string displayName = validationContext.DisplayName;

        if (value is not string selectedValue)
        {
            return new ValidationResult($"{displayName} is invalid.");
        }

        if (Regex.IsMatch(selectedValue, RegexPatterns.Uppercase))
        {
            return new ValidationResult($"{displayName} must not contain uppercase letters.");
        }

        if (Regex.IsMatch(selectedValue, RegexPatterns.SpecialChar))
        {
            return new ValidationResult($"{displayName} must not contain special characters.");
        }

        if (Regex.IsMatch(selectedValue, RegexPatterns.Whitespace))
        {
            return new ValidationResult($"{displayName} must not contain whitespace.");
        }

        return ValidationResult.Success;
    }
}
