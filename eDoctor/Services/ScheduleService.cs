using eDoctor.Areas.Doctor.Models.Dtos.Schedule.Queries;
using eDoctor.Data;
using eDoctor.Enums;
using eDoctor.Interfaces;
using eDoctor.Models;
using eDoctor.Results;
using Microsoft.EntityFrameworkCore;

namespace eDoctor.Services;

public class ScheduleService : IScheduleService
{
    private readonly ApplicationDbContext _context;

    public ScheduleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> AddAsync(CreateQueryDto dto)
    {
        DateTime bufferedStart = dto.StartTime.AddMinutes(-5);
        DateTime bufferedEnd = dto.EndTime.AddMinutes(5);

        if (await _context.Schedules
                .Where(s => s.DoctorId == dto.DoctorId)
                .AnyAsync(s => s.StartTime < bufferedEnd && s.EndTime > bufferedStart))
        {
            return Result.Failure("Schedule overlaps. Please keep at least a 5-minute gap.");
        }

        Schedule schedule = new Schedule
        {
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Status = ScheduleStatus.CREATED,
            DoctorId = dto.DoctorId
        };

        await _context.Schedules.AddAsync(schedule);
        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
