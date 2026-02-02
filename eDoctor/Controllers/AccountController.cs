using eDoctor.Attributes;
using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.User;
using eDoctor.Models.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public AccountController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
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

        if (!(await _userService.CheckPasswordAsync(vm.LoginName, vm.Password)))
        {
            ModelState.AddModelError("", "Invalid login name or password.");

            return View(vm);
        }

        await _authService.LoginAsync(vm.LoginName, RoleTypes.User);

        TempData.SetAlert("Login successful!", AlertTypes.Success);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [OnlyAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [OnlyAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        if (await _userService.ExistsByLoginNameAsync(vm.LoginName))
        {
            string key = nameof(RegisterViewModel.LoginName);
            string displayName = DisplayHelper.GetDislayName<RegisterViewModel>(key);

            ModelState.AddModelError(key, $"{displayName} already exists.");

            return View(vm);
        }

        RegisterDto dto = new RegisterDto
        {
            FullName = vm.FullName,
            BirthDate = vm.BirthDate,
            Sex = vm.Sex,
            LoginName = vm.LoginName,
            Password = vm.Password
        };

        await _userService.AddAsync(dto);

        TempData.SetAlert("Registration successful!", AlertTypes.Success);

        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.User)]
    public IActionResult Profile()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        
        return RedirectToAction("Login", "Account");
    }
}
