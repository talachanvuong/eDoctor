using eDoctor.Areas.Doctor.Models.Dtos.Schedule;
using eDoctor.Areas.Doctor.Models.Dtos.Schedule.Queries;
using eDoctor.Areas.Doctor.Models.ViewModels.Schedule;
using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Results;
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
    public async Task<IActionResult> Index(SchedulesViewModel vm)
    {
        SchedulesQueryDto dto = new SchedulesQueryDto
        {
            Date = vm.Date,
            Status = vm.Status
        };

        SchedulesDto schedules = await _scheduleService.GetSchedulesAsync(dto);

        vm.Schedules = schedules.Schedules.Select(s => new ScheduleViewModel
        {
            ScheduleId = s.ScheduleId,
            Time = DateTimeHelper.ConvertToString(s.StartTime, s.EndTime),
            Status = s.Status.ConvertToString()
        });

        return View(vm);
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

    [HttpGet]
    [Authorize(Roles = RoleTypes.Doctor)]
    public async Task<IActionResult> DetailSchedule(DetailViewModel vm)
    {
        DetailQueryDto dto = new DetailQueryDto
        {
            ScheduleId = vm.ScheduleId,
            DoctorId = User.GetId()
        };

        Result<DetailDto> result = await _scheduleService.GetDetailAsync(dto);

        if (!result.IsSuccess)
        {
            TempData.SetAlert(result.Error!, AlertTypes.Danger);

            return RedirectToAction("Index", "Home");
        }

        DetailDto detail = result.Value!;

        vm.UserId = detail.UserId;
        vm.Room = detail.Room;
        vm.Time = DateTimeHelper.ConvertToString(detail.StartTime, detail.EndTime);
        vm.Status = detail.Status.ConvertToString();
        vm.Patient = detail.Patient;

        return View(vm);
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.Doctor)]
    public async Task<IActionResult> CancelSchedule(CancelViewModel vm)
    {
        CancelQueryDto dto = new CancelQueryDto
        {
            ScheduleId = vm.ScheduleId,
            DoctorId = User.GetId()
        };

        Result result = await _scheduleService.CancelAsync(dto);

        if (!result.IsSuccess)
        {
            TempData.SetAlert(result.Error!, AlertTypes.Danger);

            return RedirectToAction("Index", "Home");
        }

        TempData.SetAlert("Cancel successfully!", AlertTypes.Success);

        return RedirectToAction("DetailSchedule", "Home", new { vm.ScheduleId });
    }
}
