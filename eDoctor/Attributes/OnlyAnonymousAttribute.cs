using eDoctor.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace eDoctor.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class OnlyAnonymousAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ClaimsPrincipal principal = context.HttpContext.User;

        if (principal.IsInRole(RoleTypes.User))
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
