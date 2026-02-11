using eDoctor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Areas.Doctor.Controllers;

[Area("Doctor")]
public class HomeController : Controller
{
    [HttpGet]
    [Authorize(Roles = RoleTypes.Doctor)]
    public IActionResult Index()
    {
        return View();
    }
}
