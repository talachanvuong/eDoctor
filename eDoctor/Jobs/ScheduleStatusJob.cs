using eDoctor.Data;
using eDoctor.Enums;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace eDoctor.Jobs;

public class ScheduleStatusJob : IJob
{
    private readonly ApplicationDbContext _context;

    public ScheduleStatusJob(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        DateTime now = DateTime.Now;

        await _context.Schedules
            .Where(s => s.StartTime <= now && s.Status == ScheduleStatus.CREATED)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.Status, ScheduleStatus.CANCELLED));

        await _context.Schedules
            .Where(s => s.StartTime <= now && s.EndTime > now && s.Status == ScheduleStatus.ORDERED)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.Status, ScheduleStatus.ONGOING));

        await _context.Schedules
            .Where(s => s.EndTime <= now && s.Status == ScheduleStatus.ONGOING)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.Status, ScheduleStatus.COMPLETED));
    }
}
