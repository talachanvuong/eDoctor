using eDoctor.Data;
using eDoctor.Enums;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Notification.Queries;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace eDoctor.Jobs;

public class ScheduleStatusJob : IJob
{
    private readonly ApplicationDbContext _context;
    private readonly INotificationService _notificationService;

    public ScheduleStatusJob(ApplicationDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        DateTime now = DateTime.Now;

        await _context.Schedules
            .Where(s => s.StartTime <= now && s.Status == ScheduleStatus.CREATED)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.Status, ScheduleStatus.CANCELLED));

        var schedules = await _context.Schedules
            .Where(s => s.StartTime <= now && s.EndTime > now && s.Status == ScheduleStatus.ORDERED)
            .Select(s => new { s.ScheduleId, s.Room })
            .ToListAsync();

        await _context.Schedules
            .Where(s => s.StartTime <= now && s.EndTime > now && s.Status == ScheduleStatus.ORDERED)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.Status, ScheduleStatus.ONGOING));

        foreach (var schedule in schedules)
        {
            await _notificationService.SendAsync(new SendQueryDto
            {
                ScheduleId = schedule.ScheduleId,
                NotificationType = NotificationType.MEETING_DOCTOR,
                Reference = $"/Doctor/Meeting/Join?Room={schedule.Room}"
            });

            await _notificationService.SendAsync(new SendQueryDto
            {
                ScheduleId = schedule.ScheduleId,
                NotificationType = NotificationType.MEETING_USER,
                Reference = $"/Meeting/Join?Room={schedule.Room}"
            });
        }

        await _context.Schedules
            .Where(s => s.EndTime <= now && s.Status == ScheduleStatus.ONGOING)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.Status, ScheduleStatus.COMPLETED));
    }
}
