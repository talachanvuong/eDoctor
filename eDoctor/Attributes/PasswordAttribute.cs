using eDoctor.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace eDoctor.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class PasswordAttribute : ValidationAttribute
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

        if (!Regex.IsMatch(selectedValue, RegexPatterns.Lowercase))
        {
            return new ValidationResult($"{displayName} must contain at least one lowercase letter.");
        }

        if (!Regex.IsMatch(selectedValue, RegexPatterns.Uppercase))
        {
            return new ValidationResult($"{displayName} must contain at least one uppercase letter.");
        }

        if (!Regex.IsMatch(selectedValue, RegexPatterns.Digit))
        {
            return new ValidationResult($"{displayName} must contain at least one digit.");
        }

        if (!Regex.IsMatch(selectedValue, RegexPatterns.SpecialChar))
        {
            return new ValidationResult($"{displayName} must contain at least one special character.");
        }

        if (Regex.IsMatch(selectedValue, RegexPatterns.Whitespace))
        {
            return new ValidationResult($"{displayName} must not contain whitespace.");
        }

        return ValidationResult.Success;
    }
}
