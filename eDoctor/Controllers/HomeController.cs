using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Doctor;
using eDoctor.Models.Dtos.Doctor.Fallbacks;
using eDoctor.Models.Dtos.Doctor.Queries;
using eDoctor.Models.ViewModels.Doctor;
using eDoctor.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Controllers;

public class HomeController : Controller
{
    private readonly IDoctorService _doctorService;
    private readonly IScheduleService _scheduleService;

    public HomeController(IDoctorService doctorService, IScheduleService scheduleService)
    {
        _doctorService = doctorService;
        _scheduleService = scheduleService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Doctors(DoctorsViewModel vm)
    {
        DoctorsQueryDto dto = new DoctorsQueryDto
        {
            DepartmentId = vm.DepartmentId
        };

        Result<DoctorsDto, DoctorsFallbackDto> result = await _doctorService.GetByDepartmentAsync(dto);

        if (!result.IsSuccess)
        {
            return RedirectToAction("Doctors", "Home", new
            {
                result.Fallback!.DepartmentId
            });
        }

        DoctorsDto value = result.Value!;

        vm.Departments = value.Departments.Select(d => new DepartmentViewModel
        {
            DepartmentId = d.DepartmentId,
            DepartmentName = d.DepartmentName
        });

        vm.Doctors = value.Doctors.Select(d => new BriefViewModel
        {
            DoctorId = d.DoctorId,
            Avatar = d.Avatar.ConvertToString(),
            FullName = d.FullName
        });

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> AboutDoctor(AboutViewModel vm)
    {
        AboutQueryDto dto = new AboutQueryDto
        {
            DoctorId = vm.DoctorId
        };

        Result<AboutDto, AboutFallbackDto> result = await _doctorService.GetAboutAsync(dto);

        if (!result.IsSuccess)
        {
            return RedirectToAction("AboutDoctor", "Home", new
            {
                result.Fallback!.DoctorId
            });
        }

        AboutDto value = result.Value!;
        DetailDto detail = value.Detail;

        vm.Detail = new DetailViewModel
        {
            FullName = detail.FullName,
            RankCode = detail.RankCode.ConvertToString(),
            YearsOfExperience = detail.YearsOfExperience,
            Avatar = detail.Avatar.ConvertToString(),
            DepartmentName = detail.DepartmentName,
            Introductions = detail.Introductions.Select(i => new IntroductionViewModel
            {
                SectionTitle = i.SectionTitle,
                Content = i.Content
            })
        };

        vm.Others = value.Others.Select(d => new OtherViewModel
        {
            DoctorId = d.DoctorId,
            Avatar = d.Avatar.ConvertToString(),
            FullName = d.FullName
        });

        return View(vm);
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
            Time = DateTimeHelper.ConvertToString(s.StartTime, s.EndTime),
        });

        return View(vm);
    }
}
