using eDoctor.Attributes;
using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.User;
using eDoctor.Models.Dtos.User.Queries;
using eDoctor.Models.ViewModels.User;
using eDoctor.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace eDoctor.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
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

        Result result = await _userService.LoginAsync(dto);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.Error!);

            return View(vm);
        }

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

        RegisterQueryDto dto = new RegisterQueryDto
        {
            FullName = vm.FullName,
            BirthDate = vm.BirthDate,
            Sex = vm.Sex,
            LoginName = vm.LoginName,
            Password = vm.Password
        };

        Result result = await _userService.AddAsync(dto);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.Error!);

            return View(vm);
        }

        TempData.SetAlert("Registration successful!", AlertTypes.Success);

        return RedirectToAction("Login", "Account");
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> Logout()
    {
        await _userService.LogoutAsync();

        TempData.SetAlert("Logout successfully!", AlertTypes.Success);

        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> Profile()
    {
        ProfileQueryDto dto = new ProfileQueryDto
        {
            UserId = User.GetId()
        };

        ProfileDto user = await _userService.GetProfileAsync(dto);

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

        UpdateQueryDto dto = new UpdateQueryDto
        {
            UserId = User.GetId(),
            FullName = vm.FullName
        };

        await _userService.UpdateAsync(dto);

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

        ChangePasswordQueryDto dto = new ChangePasswordQueryDto
        {
            UserId = User.GetId(),
            OldPassword = vm.OldPassword,
            NewPassword = vm.NewPassword
        };

        Result result = await _userService.ChangePasswordAsync(dto);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.Error!);

            return View(vm);
        }

        TempData.SetAlert("Change password successfully!", AlertTypes.Success);

        return RedirectToAction("Login", "Account");
    }
}
