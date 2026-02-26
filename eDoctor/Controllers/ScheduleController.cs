using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Schedule;
using eDoctor.Models.Dtos.Schedule.Queries;
using eDoctor.Models.ViewModels.Schedule;
using eDoctor.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Controllers;

public class ScheduleController : Controller
{
    private readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> Schedules(SchedulesViewModel vm)
    {
        SchedulesQueryDto dto = new SchedulesQueryDto
        {
            DoctorId = vm.DoctorId,
            Date = vm.Date,
            UserId = User.GetId()
        };

        Result<SchedulesDto> result = await _scheduleService.GetSchedulesAsync(dto);

        if (!result.IsSuccess)
        {
            TempData.SetAlert(result.Error!, AlertTypes.Danger);

            return RedirectToAction("Doctors", "Home");
        }

        SchedulesDto schedules = result.Value!;

        vm.Schedules = schedules.Schedules.Select(s => new ScheduleViewModel
        {
            ScheduleId = s.ScheduleId,
            Time = DateTimeHelper.ConvertToString(s.StartTime, s.EndTime)
        });

        return View(vm);
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> MySchedules(MySchedulesViewModel vm)
    {
        MySchedulesQueryDto dto = new MySchedulesQueryDto
        {
            UserId = User.GetId()
        };

        MySchedulesDto schedules = await _scheduleService.GetMySchedulesAsync(dto);

        vm.Schedules = schedules.Schedules.Select(s => new MyScheduleViewModel
        {
            ScheduleId = s.ScheduleId,
            Time = DateTimeHelper.ConvertToString(s.StartTime, s.EndTime),
            Status = s.Status.ConvertToString()
        });

        return View(vm);
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> MyDetailSchedule(MyDetailScheduleViewModel vm)
    {
        MyDetailScheduleQueryDto dto = new MyDetailScheduleQueryDto
        {
            ScheduleId = vm.ScheduleId,
            UserId = User.GetId()
        };

        Result<MyDetailScheduleDto> result = await _scheduleService.GetMyDetailScheduleAsync(dto);

        if (!result.IsSuccess)
        {
            TempData.SetAlert(result.Error!, AlertTypes.Danger);

            return RedirectToAction("MySchedules", "Schedule");
        }

        MyDetailScheduleDto detail = result.Value!;

        vm.Room = detail.Room;
        vm.Time = DateTimeHelper.ConvertToString(detail.StartTime, detail.EndTime);
        vm.Status = detail.Status.ConvertToString();
        vm.Doctor = $"{detail.RankCode.ConvertToString()} {detail.FullName}";

        return View(vm);
    }
}
