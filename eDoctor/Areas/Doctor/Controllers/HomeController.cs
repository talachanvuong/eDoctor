using eDoctor.Areas.Doctor.Models.Dtos.Schedule.Queries;
using eDoctor.Areas.Doctor.Models.ViewModels.Schedule;
using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Results;
using eDoctor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Areas.Doctor.Controllers;

[Area("Doctor")]
public class HomeController : Controller
{
    private readonly IScheduleService _scheduleService;

    public HomeController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.Doctor)]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.Doctor)]
    public IActionResult CreateSchedule()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.Doctor)]
    public async Task<IActionResult> CreateSchedule(CreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        CreateQueryDto dto = new CreateQueryDto
        {
            DoctorId = User.GetId(),
            StartTime = vm.StartTime,
            EndTime = vm.StartTime + TimeSpan.FromHours(1)
        };

        Result result = await _scheduleService.AddAsync(dto);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.Error!);

            return View(vm);
        }

        TempData.SetAlert("Create new appointment successfully!", AlertTypes.Success);

        return RedirectToAction("Index", "Home");
    }
}
