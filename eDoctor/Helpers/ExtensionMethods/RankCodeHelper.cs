using eDoctor.Enums;

namespace eDoctor.Helpers.ExtensionMethods;

public static class RankCodeHelper
{
    public static string ConvertToString(this RankCode rankCode)
    {
        return rankCode switch
        {
            RankCode.DOCTOR => "Dr.",
            RankCode.MASTER_OF_SCIENCE => "MS",
            RankCode.DOCTOR_OF_PHILOSOPHY => "PhD",
            RankCode.PROFESSOR => "Prof.",
            _ => "None"
        };
    }
}
