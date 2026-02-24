namespace eDoctor.Models.ViewModels.Schedule;

public class MyDetailScheduleViewModel
{
    public int ScheduleId { get; set; }
    public string Room { get; set; } = null!;
    public string Time { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Doctor { get; set; } = null!;
}
