using eDoctor.Attributes;
using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Models;
using eDoctor.Models.Dtos.User;
using eDoctor.Models.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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

        User? user = await _userService.CheckPasswordAsync(vm.LoginName, vm.Password);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid login name or password.");

            return View(vm);
        }

        await _authService.LoginAsync(user.UserId, RoleTypes.User);

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

    [HttpPost]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();

        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> Profile()
    {
        int userId = User.GetId();

        User user = await _userService.GetCurrentAsync(userId);

        ProfileViewModel vm = new ProfileViewModel
        {
            FullName = user.FullName,
            BirthDate = user.BirthDate.ToString("d", CultureInfo.GetCultureInfo("en-US")),
            Sex = user.Sex ? "Female" : "Male"
        };

        return View(vm);
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> Profile(ProfileViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        int userId = User.GetId();

        UpdateDto dto = new UpdateDto
        {
            FullName = vm.FullName
        };

        await _userService.UpdateAsync(userId, dto);

        TempData.SetAlert("Update successfully!", AlertTypes.Success);

        return RedirectToAction("Profile", "Account");
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.User)]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        int userId = User.GetId();

        if (!await _userService.CheckPasswordAsync(userId, vm.OldPassword))
        {
            ModelState.AddModelError(nameof(ChangePasswordViewModel.OldPassword), "Wrong old password.");

            return View(vm);
        }

        ChangePasswordDto dto = new ChangePasswordDto
        {
            NewPassword = vm.NewPassword
        };

        await _userService.ChangePasswordAsync(userId, dto);
        await _authService.LogoutAsync();

        TempData.SetAlert("Change password successfully!", AlertTypes.Success);

        return RedirectToAction("Login", "Account");
    }
}
