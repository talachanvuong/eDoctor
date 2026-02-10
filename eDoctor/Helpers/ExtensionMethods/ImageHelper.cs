namespace eDoctor.Helpers.ExtensionMethods;

public static class ImageHelper
{
    public static string ConvertToString(this byte[] bytes)
    {
        return $"data:image/png;base64,{Convert.ToBase64String(bytes)}";
    }
}
