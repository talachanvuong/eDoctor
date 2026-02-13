using eDoctor.Enums;

namespace eDoctor.Helpers.ExtensionMethods;

public static class RankCodeHelper
{
    public static string ConvertToString(this RankCode rankCode)
    {
        return rankCode switch
        {
            RankCode.Doctor => "Dr.",
            RankCode.MasterOfScience => "MS",
            RankCode.DoctorOfPhilosophy => "PhD",
            RankCode.Professor => "Prof.",
            _ => "None"
        };
    }
}
