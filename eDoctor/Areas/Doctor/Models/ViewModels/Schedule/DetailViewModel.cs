namespace eDoctor.Areas.Doctor.Models.ViewModels.Schedule;

public class DetailViewModel
{
    public int ScheduleId { get; set; }
    public int? UserId { get; set; }
    public string? Room { get; set; }
    public string Time { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? Patient { get; set; }
}
