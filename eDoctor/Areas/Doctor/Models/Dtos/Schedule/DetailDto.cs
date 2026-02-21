using eDoctor.Enums;

namespace eDoctor.Areas.Doctor.Models.Dtos.Schedule;

public class DetailDto
{
    public int? UserId { get; set; }
    public string? Room { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ScheduleStatus Status { get; set; }
    public string? Patient { get; set; }
}
