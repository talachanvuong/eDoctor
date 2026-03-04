using eDoctor.Data;
using eDoctor.Enums;
using eDoctor.Helpers;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Meeting.Queries;
using eDoctor.Results;
using Microsoft.EntityFrameworkCore;

namespace eDoctor.Services;

public class MeetingService : IMeetingService
{
    private readonly ApplicationDbContext _context;

    public MeetingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> JoinAsync(RoomQueryDto dto)
    {
        var query = _context.Schedules.Where(s => s.Room == dto.Room && s.Status == ScheduleStatus.ONGOING);

        if (dto.Role == RoleTypes.User)
        {
            query = query.Where(s => s.UserId == dto.Id);
        }
        else if (dto.Role == RoleTypes.Doctor)
        {
            query = query.Where(s => s.DoctorId == dto.Id);
        }

        var schedule = await query.FirstOrDefaultAsync();

        if (schedule == null)
        {
            return Result.Failure("Cannot join room.");
        }

        return Result.Success();
    }
}
