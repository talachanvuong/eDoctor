using eDoctor.Data;
using eDoctor.Enums;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Hubs;
using eDoctor.Interfaces;
using eDoctor.Models;
using eDoctor.Models.Dtos.Notification;
using eDoctor.Models.Dtos.Notification.Queries;
using eDoctor.Results;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace eDoctor.Services;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<NotificationHub> _notificationHub;

    public NotificationService(ApplicationDbContext context, IHubContext<NotificationHub> notificationHub)
    {
        _context = context;
        _notificationHub = notificationHub;
    }

    public async Task SendAsync(SendQueryDto dto)
    {
        Schedule schedule = await _context.Schedules
            .Include(s => s.Doctor)
            .Include(s => s.User)
            .FirstAsync(s => s.ScheduleId == dto.ScheduleId);

        switch (dto.NotificationType)
        {
            case NotificationType.BOOK_SCHEDULE:
                Notification notification = new Notification
                {
                    Receiver = schedule.DoctorId,
                    ReceiverType = ActorType.DOCTOR,
                    Content = $"{schedule.User!.FullName} has booked your schedule at {schedule.StartTime:HH:mm}.",
                    Reference = dto.Reference,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };

                await _context.Notifications.AddAsync(notification);

                await _context.SaveChangesAsync();

                await _notificationHub.Clients
                    .Group($"Doctor-{schedule.DoctorId}")
                    .SendAsync("ReceiveNotification", new NotificationDto
                    {
                        NotificationId = notification.NotificationId,
                        Content = notification.Content,
                        Reference = notification.Reference,
                        IsRead = notification.IsRead,
                        CreatedAt = notification.CreatedAt
                    });

                break;

            case NotificationType.CREATE_MEDICAL_RECORD:
                Notification notification1 = new Notification
                {
                    Receiver = (int)schedule.UserId!,
                    ReceiverType = ActorType.USER,
                    Content = $"{schedule.Doctor.RankCode.ConvertToString()} {schedule.Doctor.FullName} has created a medical record.",
                    Reference = dto.Reference,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };

                await _context.Notifications.AddAsync(notification1);

                await _context.SaveChangesAsync();

                await _notificationHub.Clients
                    .Group($"User-{schedule.UserId}")
                    .SendAsync("ReceiveNotification", new NotificationDto
                    {
                        NotificationId = notification1.NotificationId,
                        Content = notification1.Content,
                        Reference = notification1.Reference,
                        IsRead = notification1.IsRead,
                        CreatedAt = notification1.CreatedAt
                    });

                break;

            case NotificationType.MEETING_DOCTOR:
                Notification notification2 = new Notification
                {
                    Receiver = schedule.DoctorId,
                    ReceiverType = ActorType.DOCTOR,
                    Content = $"Your appointment with {schedule.User!.FullName} is starting now.",
                    Reference = dto.Reference,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };

                await _context.Notifications.AddAsync(notification2);

                await _context.SaveChangesAsync();

                await _notificationHub.Clients
                    .Group($"Doctor-{schedule.DoctorId}")
                    .SendAsync("ReceiveNotification", new NotificationDto
                    {
                        NotificationId = notification2.NotificationId,
                        Content = notification2.Content,
                        Reference = notification2.Reference,
                        IsRead = notification2.IsRead,
                        CreatedAt = notification2.CreatedAt
                    });

                break;

            case NotificationType.MEETING_USER:
                Notification notification3 = new Notification
                {
                    Receiver = (int)schedule.UserId!,
                    ReceiverType = ActorType.USER,
                    Content = $"Your appointment with {schedule.Doctor.RankCode.ConvertToString()} {schedule.Doctor.FullName} is starting now.",
                    Reference = dto.Reference,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };

                await _context.Notifications.AddAsync(notification3);

                await _context.SaveChangesAsync();

                await _notificationHub.Clients
                    .Group($"User-{schedule.UserId}")
                    .SendAsync("ReceiveNotification", new NotificationDto
                    {
                        NotificationId = notification3.NotificationId,
                        Content = notification3.Content,
                        Reference = notification3.Reference,
                        IsRead = notification3.IsRead,
                        CreatedAt = notification3.CreatedAt
                    });

                break;
        }
    }

    public async Task<MyDto> GetMyAsync(MyQueryDto dto)
    {
        IEnumerable<NotificationDto> notifications = await _context.Notifications
            .Where(n => n.Receiver == dto.Id && n.ReceiverType == dto.ActorType)
            .OrderByDescending(n => n.CreatedAt)
            .Take(99)
            .Select(n => new NotificationDto
            {
                NotificationId = n.NotificationId,
                Content = n.Content,
                Reference = n.Reference,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            })
            .ToListAsync();

        return new MyDto
        {
            Notifications = notifications
        };
    }

    public async Task<Result> ReadAsync(ReadQueryDto dto)
    {
        var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.NotificationId == dto.NotificationId && n.Receiver == dto.Receiver && n.ReceiverType == dto.ActorType);

        if (notification == null)
        {
            return Result.Failure("Notification not found.");
        }

        notification.IsRead = true;

        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
