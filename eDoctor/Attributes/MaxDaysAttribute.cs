using System.ComponentModel.DataAnnotations;

namespace eDoctor.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class MaxDaysAttribute : ValidationAttribute
{
    private readonly int _days;

    public MaxDaysAttribute(int days)
    {
        _days = days;
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

        DateTime max = DateTime.Now + TimeSpan.FromDays(_days);

        if (max < selectedValue)
        {
            return new ValidationResult($"{validationContext.DisplayName} cannot be more than {_days} days from today.");
        }

        return ValidationResult.Success;
    }
}
