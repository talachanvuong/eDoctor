using System.ComponentModel.DataAnnotations;

namespace eDoctor.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class MinMinutesAttribute : ValidationAttribute
{
    private readonly int _minutes;

    public MinMinutesAttribute(int minutes)
    {
        _minutes = minutes;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is not DateTime selectedValue)
        {
            return new ValidationResult($"{validationContext.DisplayName} is invalid.");
        }

        DateTime min = DateTime.Now + TimeSpan.FromMinutes(_minutes);

        if (min > selectedValue)
        {
            return new ValidationResult($"{validationContext.DisplayName} must be at least {_minutes} minutes from now.");
        }

        return ValidationResult.Success;
    }
}
