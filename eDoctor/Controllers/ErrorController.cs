using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Controllers;

public class ErrorController : Controller
{
    public IActionResult Error()
    {
        return View();
    }
}
