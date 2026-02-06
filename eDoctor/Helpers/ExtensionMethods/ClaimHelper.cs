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
}
