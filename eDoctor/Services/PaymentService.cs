using eDoctor.Data;
using eDoctor.Enums;
using eDoctor.Interfaces;
using eDoctor.Models;
using eDoctor.Models.Dtos.Payment;
using eDoctor.Models.Dtos.Payment.Queries;
using eDoctor.Results;
using Microsoft.EntityFrameworkCore;
using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Controllers;
using PaypalServerSdk.Standard.Exceptions;
using PaypalServerSdk.Standard.Http.Response;
using PaypalServerSdk.Standard.Models;

namespace eDoctor.Services;

public class PaymentService : IPaymentService
{
    private readonly ApplicationDbContext _context;
    private readonly PaypalServerSdkClient _paypal;

    public PaymentService(ApplicationDbContext context, PaypalServerSdkClient paypal)
    {
        _context = context;
        _paypal = paypal;
    }

    public async Task<Result<BillDto>> GetBillAsync(BillQueryDto dto)
    {
        if (!await _context.Schedules.AnyAsync(s => s.ScheduleId == dto.ScheduleId))
        {
            return Result<BillDto>.Failure("Schedule not found.");
        }

        var cutoff = DateTime.Now + TimeSpan.FromMinutes(15);

        var patientSchedules = _context.Schedules
            .Where(s => s.UserId == dto.UserId && (s.Status == ScheduleStatus.ORDERED || s.Status == ScheduleStatus.ONGOING));

        var schedule = await _context.Schedules
            .Include(s => s.Doctor)
            .Where(s => s.ScheduleId == dto.ScheduleId && s.Status == ScheduleStatus.CREATED && s.StartTime > cutoff && !patientSchedules
                .Any(p => p.StartTime < s.EndTime && p.EndTime > s.StartTime))
            .FirstOrDefaultAsync();

        if (schedule == null)
        {
            return Result<BillDto>.Failure("Schedule not available.");
        }

        var service = await _context.Services.FirstAsync();

        BillDto value = new BillDto
        {
            Services = [new ServiceDto {
                ServiceName = service.ServiceName,
                Price = service.Price
            }],
            RankCode = schedule.Doctor.RankCode,
            FullName = schedule.Doctor.FullName,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime
        };

        return Result<BillDto>.Success(value);
    }

    public async Task<Result<CreateOrderDto>> CreateOrderAsync(CreateOrderQueryDto dto)
    {
        OrdersController ordersController = _paypal.OrdersController;

        CreateOrderInput createOrderInput = new CreateOrderInput
        {
            Body = new OrderRequest
            {
                Intent = CheckoutPaymentIntent.Capture,
                PurchaseUnits = [new PurchaseUnitRequest
                {
                    Amount = new AmountWithBreakdown
                    {
                        CurrencyCode = "USD",
                        MValue = dto.Total.ToString()
                    }
                }]
            },
            Prefer = "return=minimal"
        };

        try
        {
            ApiResponse<Order> result = await ordersController.CreateOrderAsync(createOrderInput);

            CreateOrderDto value = new CreateOrderDto
            {
                OrderId = result.Data.Id
            };

            return Result<CreateOrderDto>.Success(value);
        }
        catch (ApiException e)
        {
            return Result<CreateOrderDto>.Failure(e.Message);
        }
    }

    public async Task<Result> CaptureAsync(CaptureQueryDto dto)
    {
        OrdersController ordersController = _paypal.OrdersController;

        CaptureOrderInput captureOrderInput = new CaptureOrderInput
        {
            Id = dto.OrderId,
            Prefer = "return=minimal"
        };

        try
        {
            ApiResponse<Order> result = await ordersController.CaptureOrderAsync(captureOrderInput);

            Schedule schedule = await _context.Schedules.FirstAsync(s => s.ScheduleId == dto.ScheduleId);

            Invoice invoice = new Invoice
            {
                OrderId = dto.OrderId,
                CreatedAt = DateTime.Now
            };

            foreach (var service in dto.Services)
            {
                invoice.DetailInvoices.Add(new DetailInvoice
                {
                    ServiceName = service.ServiceName,
                    Price = service.Price
                });
            }

            // Create room

            // Update room to schedule
            schedule.Status = ScheduleStatus.ORDERED;
            schedule.UserId = dto.UserId;
            schedule.Invoice = invoice;

            await _context.SaveChangesAsync();

            return Result<CreateOrderDto>.Success();
        }
        catch (ApiException e)
        {
            return Result<CreateOrderDto>.Failure(e.Message);
        }
    }
}
