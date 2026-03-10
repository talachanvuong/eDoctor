using eDoctor.Enums;

namespace eDoctor.Models.Dtos.Notification.Queries;

public class SendQueryDto
{
    public int ScheduleId { get; set; }
    public NotificationType NotificationType { get; set; }
    public string? Reference { get; set; }
}
