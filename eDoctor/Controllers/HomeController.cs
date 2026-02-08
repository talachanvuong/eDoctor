using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Department;
using eDoctor.Models.Dtos.Doctor;
using eDoctor.Models.ViewModels.Compound;
using eDoctor.Models.ViewModels.Department;
using eDoctor.Models.ViewModels.Doctor;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Controllers;

public class HomeController : Controller
{
    private readonly IDepartmentService _departmentService;
    private readonly IDoctorService _doctorService;

    public HomeController(IDepartmentService departmentService, IDoctorService doctorService)
    {
        _departmentService = departmentService;
        _doctorService = doctorService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Doctors(DepartmentBriefViewModel vm)
    {
        IEnumerable<DepartmentDto> departments = await _departmentService.GetAllAsync();

        vm.Departments = departments.Select(d => new DepartmentViewModel
        {
            DepartmentId = d.DepartmentId,
            DepartmentName = d.DepartmentName
        });

        IEnumerable<BriefDto> doctors = await _doctorService.GetByDepartmentAsync(vm.DepartmentId);

        vm.Doctors = doctors.Select(d => new BriefViewModel
        {
            DoctorId = d.DoctorId,
            Avatar = $"data:image/png;base64,{Convert.ToBase64String(d.Avatar)}",
            FullName = d.FullName
        });

        return View(vm);
    }
}
