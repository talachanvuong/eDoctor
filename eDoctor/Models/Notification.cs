using eDoctor.Enums;
using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models;

public class Notification
{
    [Key]
    public int NotificationId { get; set; }

    public int Receiver { get; set; }

    public ActorType ReceiverType { get; set; }

    [MaxLength(2048)]
    public string Content { get; set; } = null!;

    [MaxLength(2048)]
    public string? Reference { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }
}
