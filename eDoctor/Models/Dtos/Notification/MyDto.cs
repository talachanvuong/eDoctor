namespace eDoctor.Models.Dtos.Notification;

public class MyDto
{
    public IEnumerable<NotificationDto> Notifications { get; set; } = null!;
}

public class NotificationDto
{
    public int NotificationId { get; set; }
    public string Content { get; set; } = null!;
    public string? Reference { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
