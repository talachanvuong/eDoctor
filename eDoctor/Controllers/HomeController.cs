using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Doctor;
using eDoctor.Models.Dtos.Doctor.Fallbacks;
using eDoctor.Models.Dtos.Doctor.Queries;
using eDoctor.Models.Dtos.Payment;
using eDoctor.Models.Dtos.Payment.Queries;
using eDoctor.Models.Dtos.Schedule;
using eDoctor.Models.Dtos.Schedule.Queries;
using eDoctor.Models.ViewModels.Doctor;
using eDoctor.Models.ViewModels.Payment;
using eDoctor.Models.ViewModels.Schedule;
using eDoctor.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Controllers;

public class HomeController : Controller
{
    private readonly IDoctorService _doctorService;
    private readonly IScheduleService _scheduleService;
    private readonly IPaymentService _paymentService;
    private readonly IConfiguration _configuration;

    public HomeController(IDoctorService doctorService, IScheduleService scheduleService, IPaymentService paymentService, IConfiguration configuration)
    {
        _doctorService = doctorService;
        _scheduleService = scheduleService;
        _paymentService = paymentService;
        _configuration = configuration;
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
            Time = DateTimeHelper.ConvertToString(s.StartTime, s.EndTime)
        });

        return View(vm);
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> Bill(BillViewModel vm)
    {
        BillQueryDto dto = new BillQueryDto
        {
            ScheduleId = vm.ScheduleId,
            UserId = User.GetId()
        };

        Result<BillDto> result = await _paymentService.GetBillAsync(dto);

        if (!result.IsSuccess)
        {
            TempData.SetAlert(result.Error!, AlertTypes.Danger);

            return RedirectToAction("Doctors", "Home");
        }

        BillDto bill = result.Value!;

        vm.Services = bill.Services.Select(s => new ServiceViewModel
        {
            ServiceName = s.ServiceName,
            Price = s.Price
        });

        vm.Total = bill.Services.Sum(s => s.Price);
        vm.Note = $"Meeting with {bill.RankCode.ConvertToString()} {bill.FullName} on {DateTimeHelper.ConvertToString(bill.StartTime, bill.EndTime)}.";

        ViewBag.ClientId = _configuration["PayPal:OAuthClientId"];

        return View(vm);
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderViewModel vm)
    {
        CreateOrderQueryDto dto = new CreateOrderQueryDto
        {
            Total = vm.Total
        };

        Result<CreateOrderDto> result = await _paymentService.CreateOrderAsync(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(new
            {
                message = result.Error
            });
        }

        return Ok(new
        {
            data = result.Value
        });
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> Capture([FromBody] CaptureViewModel vm)
    {
        CaptureQueryDto dto = new CaptureQueryDto
        {
            ScheduleId = vm.ScheduleId,
            UserId = User.GetId(),
            OrderId = vm.OrderId,
            Services = vm.Services.Select(s => new ServiceQueryDto
            {
                ServiceName = s.ServiceName,
                Price = s.Price
            })
        };

        Result result = await _paymentService.CaptureAsync(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(new
            {
                message = result.Error
            });
        }

        return Ok();
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
}
