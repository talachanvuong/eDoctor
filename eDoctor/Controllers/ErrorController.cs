using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Controllers;

public class ErrorController : Controller
{
    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }
}
