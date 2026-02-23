using eDoctor.Data;
using eDoctor.Enums;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Payment;
using eDoctor.Models.Dtos.Payment.Queries;
using eDoctor.Results;
using Microsoft.EntityFrameworkCore;

namespace eDoctor.Services;

public class PaymentService : IPaymentService
{
    private readonly ApplicationDbContext _context;

    public PaymentService(ApplicationDbContext context)
    {
        _context = context;
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
}
