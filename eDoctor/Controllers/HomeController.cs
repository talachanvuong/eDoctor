using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Doctor;
using eDoctor.Models.Dtos.Doctor.Fallbacks;
using eDoctor.Models.Dtos.Doctor.Queries;
using eDoctor.Models.ViewModels.Doctor;
using eDoctor.Results;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Controllers;

public class HomeController : Controller
{
    private readonly IDoctorService _doctorService;

    public HomeController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
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
            Avatar = $"data:image/png;base64,{Convert.ToBase64String(d.Avatar)}",
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
            DoctorId = detail.DoctorId,
            FullName = detail.FullName,
            RankCode = detail.RankCode,
            YearsOfExperience = detail.YearsOfExperience,
            Avatar = $"data:image/png;base64,{Convert.ToBase64String(detail.Avatar)}",
            DepartmentName = detail.DepartmentName,
            Introductions = detail.Introductions.Select(i => new IntroductionViewModel
            {
                SectionTitle = i.SectionTitle,
                Content = i.Content
            })
        };

        vm.Others = value.Others.Select(d => new BriefViewModel
        {
            DoctorId = d.DoctorId,
            Avatar = $"data:image/png;base64,{Convert.ToBase64String(d.Avatar)}",
            FullName = d.FullName
        });

        return View(vm);
    }
}
