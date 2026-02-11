using eDoctor.Areas.Doctor.Models.Dtos.Doctor.Queries;
using eDoctor.Areas.Doctor.Models.ViewModels.Doctor;
using eDoctor.Attributes;
using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Areas.Doctor.Controllers;

[Area("Doctor")]
public class AccountController : Controller
{
    private readonly IDoctorService _doctorService;

    public AccountController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    [OnlyAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [OnlyAnonymous]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        LoginQueryDto dto = new LoginQueryDto
        {
            LoginName = vm.LoginName,
            Password = vm.Password
        };

        Result result = await _doctorService.LoginAsync(dto);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.Error!);

            return View(vm);
        }

        TempData.SetAlert("Login successful!", AlertTypes.Success);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.Doctor)]
    public async Task<IActionResult> Logout()
    {
        await _doctorService.LogoutAsync();

        TempData.SetAlert("Logout successfully!", AlertTypes.Success);

        return RedirectToAction("Login", "Account");
    }
}
