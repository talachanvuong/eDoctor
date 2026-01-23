using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace eDoctor.Helpers.ExtensionMethods;

public static class AlertHelper
{
    public static void SetAlert(this ITempDataDictionary tempData, string message, string type, int delay = 3000)
    {
        tempData["AlertMessage"] = message;
        tempData["AlertType"] = type;
        tempData["AlertDelay"] = delay;
    }

    public static bool HasAlert(this ITempDataDictionary tempData)
    {
        return tempData.ContainsKey("AlertMessage");
    }
}
