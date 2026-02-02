using System.Security.Claims;

namespace eDoctor.Helpers.ExtensionMethods;

public static class ClaimHelper
{
    public static string GetLoginName(this ClaimsPrincipal principal)
    {
        Claim? loginNameClaim = principal.FindFirst(ClaimTypes.Name);

        if (loginNameClaim == null)
        {
            throw new Exception("Missing login name claim");
        }

        return loginNameClaim.Value;
    }
}
