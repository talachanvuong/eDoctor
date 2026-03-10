using eDoctor.Enums;

namespace eDoctor.Models.Dtos.Notification.Queries;

public class ReadQueryDto
{
    public int NotificationId { get; set; }
    public int Receiver { get; set; }
    public ActorType ActorType { get; set; }
}
