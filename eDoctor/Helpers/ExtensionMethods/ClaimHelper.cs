using System.Security.Claims;

namespace eDoctor.Helpers.ExtensionMethods;

public static class ClaimHelper
{
    public static int GetId(this ClaimsPrincipal principal)
    {
        Claim? idClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

        if (idClaim == null)
        {
            throw new Exception("Missing id claim");
        }

        return int.Parse(idClaim.Value);
    }

    public static string GetRole(this ClaimsPrincipal principal)
    {
        Claim? roleClaim = principal.FindFirst(ClaimTypes.Role);

        if (roleClaim == null)
        {
            throw new Exception("Missing role claim");
        }

        return roleClaim.Value;
    }
}
