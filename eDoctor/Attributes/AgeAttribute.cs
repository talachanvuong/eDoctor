using System.ComponentModel.DataAnnotations;

namespace eDoctor.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class AgeAttribute : ValidationAttribute
{
    private readonly int _min;
    private readonly int _max;

    public AgeAttribute(int min, int max)
    {
        _min = min;
        _max = max;
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

        DateTime today = DateTime.Today;
        int age = (int)((today - selectedValue).TotalDays / 365.2425);

        if (age < _min || age > _max)
        {
            return new ValidationResult($"Age must be between {_min} and {_max}.");
        }

        return ValidationResult.Success;
    }
}
