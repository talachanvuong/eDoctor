using eDoctor.Areas.Doctor.Models.Dtos.Doctor;
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

    [HttpGet]
    [Authorize(Roles = RoleTypes.Doctor)]
    public async Task<IActionResult> Profile()
    {
        ProfileQueryDto dto = new ProfileQueryDto
        {
            DoctorId = User.GetId()
        };

        ProfileDto doctor = await _doctorService.GetProfileAsync(dto);
        InfoDto info = doctor.Info;

        ProfileViewModel vm = new ProfileViewModel
        {
            Info = new InfoViewModel
            {
                FullName = info.FullName,
                BirthDate = info.BirthDate,
                Gender = info.Gender,
                YearsOfExperience = info.YearsOfExperience,
                Avatar = info.Avatar.ConvertToString()
            },
            Introductions = doctor.Introductions.Select(i => new IntroductionViewModel
            {
                SectionId = i.SectionId,
                SectionTitle = i.SectionTitle,
                Content = i.Content
            }).ToList()
        };

        return View(vm);
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.Doctor)]
    public async Task<IActionResult> Profile(ProfileViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        return View(vm);
    }
}
