using System.Globalization;

namespace eDoctor.Helpers.ExtensionMethods;

public static class DateTimeHelper
{
    public static string ConvertToString(DateTime startTime, DateTime endTime)
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            "{0:HH:mm} - {1:HH:mm} ({0:ddd, MMM dd, yyyy})",
            startTime,
            endTime
        );
    }
}
