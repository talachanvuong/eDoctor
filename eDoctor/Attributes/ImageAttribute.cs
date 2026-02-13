using System.ComponentModel.DataAnnotations;

namespace eDoctor.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class ImageAttribute : ValidationAttribute
{
    private readonly long _maxSize;
    private readonly string[] _extensions;

    public ImageAttribute(long maxSize, string[] extensions)
    {
        _maxSize = maxSize;
        _extensions = extensions;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is not IFormFile selectedValue)
        {
            return new ValidationResult($"{validationContext.DisplayName} is invalid.");
        }

        if (selectedValue.Length > _maxSize)
        {
            return new ValidationResult($"File size must not exceed {_maxSize / (1024 * 1024)} MB.");
        }

        if (selectedValue.Length == 0)
        {
            return new ValidationResult("File cannot be empty.");
        }

        string extension = Path.GetExtension(selectedValue.FileName).ToLower();

        if (!_extensions.Contains(extension))
        {
            return new ValidationResult("Invalid file type.");
        }

        return ValidationResult.Success;
    }

}
