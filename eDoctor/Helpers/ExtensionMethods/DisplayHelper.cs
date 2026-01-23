using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace eDoctor.Helpers.ExtensionMethods;

public static class DisplayHelper
{
    public static string GetDislayName<T>(string propertyName)
    {
        PropertyInfo? propertyInfo = typeof(T).GetProperty(propertyName);

        if (propertyInfo == null)
        {
            throw new Exception("Property not found");
        }

        DisplayAttribute? displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();

        if (displayAttribute == null)
        {
            throw new Exception("Display attribute not found");
        }

        string? displayName = displayAttribute.Name;

        if (displayName == null)
        {
            throw new Exception("Name not found");
        }

        return displayName;
    }
}
