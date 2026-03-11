namespace eDoctor.Helpers.ExtensionMethods;

public static class DateTimeHelper
{
    public static string ConvertToString(DateTime startTime, DateTime endTime)
    {
        return $"{startTime:HH:mm} - {endTime:HH:mm} ({startTime:ddd, MMM dd, yyyy})";
    }
}
