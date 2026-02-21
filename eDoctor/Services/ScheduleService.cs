using eDoctor.Areas.Doctor.Models.Dtos.Schedule;
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

    public async Task<SchedulesDto> GetSchedulesAsync(SchedulesQueryDto dto)
    {
        var query = _context.Schedules.AsQueryable();

        if (dto.Date != null)
        {
            var startOfDay = dto.Date.Value.Date;
            var endOfDay = startOfDay.AddDays(1);

            query = query.Where(s => s.StartTime < endOfDay && s.EndTime > startOfDay);
        }

        if (dto.Status != null)
        {
            query = query.Where(s => s.Status == dto.Status);
        }

        IEnumerable<ScheduleDto> schedules = await query.OrderByDescending(s => s.StartTime)
            .Select(s => new ScheduleDto
            {
                ScheduleId = s.ScheduleId,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Status = s.Status
            })
            .ToListAsync();

        return new SchedulesDto
        {
            Schedules = schedules
        };
    }

    public async Task<Result<DetailDto>> GetDetailAsync(DetailQueryDto dto)
    {
        if (!await _context.Schedules.AnyAsync(s => s.ScheduleId == dto.ScheduleId && s.DoctorId == dto.DoctorId))
        {
            return Result<DetailDto>.Failure("Schedule not found.");
        }

        DetailDto value = await _context.Schedules
            .Where(s => s.ScheduleId == dto.ScheduleId && s.DoctorId == dto.DoctorId)
            .Select(s => new DetailDto
            {
                UserId = s.UserId,
                Room = s.Room,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Status = s.Status,
                Patient = s.User != null ? s.User.FullName : null
            }).FirstAsync();

        return Result<DetailDto>.Success(value);
    }

    public async Task<Result> CancelAsync(CancelQueryDto dto)
    {
        var affected = await _context.Schedules
            .Where(s => s.ScheduleId == dto.ScheduleId && s.DoctorId == dto.DoctorId && s.Status == ScheduleStatus.CREATED)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.Status, ScheduleStatus.CANCELLED));

        if (affected == 0)
        {
            return Result.Failure("Cancel schedule failed.");
        }

        return Result.Success();
    }
}
