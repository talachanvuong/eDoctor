using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Payment;
using eDoctor.Models.Dtos.Payment.Queries;
using eDoctor.Models.ViewModels.Payment;
using eDoctor.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Controllers;

public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    private readonly IConfiguration _configuration;

    public PaymentController(IPaymentService paymentService, IConfiguration configuration)
    {
        _paymentService = paymentService;
        _configuration = configuration;
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
    public async Task<IActionResult> Invoice(InvoiceViewModel vm)
    {
        InvoiceQueryDto dto = new InvoiceQueryDto
        {
            ScheduleId = vm.ScheduleId,
            UserId = User.GetId()
        };

        Result<InvoiceDto> result = await _paymentService.GetInvoiceAsync(dto);

        if (!result.IsSuccess)
        {
            TempData.SetAlert(result.Error!, AlertTypes.Danger);

            return RedirectToAction("Index", "Home");
        }

        InvoiceDto invoice = result.Value!;

        return File(invoice.Pdf, "application/pdf");
    }
}
