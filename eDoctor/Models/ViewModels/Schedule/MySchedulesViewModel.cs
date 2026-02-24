namespace eDoctor.Models.ViewModels.Schedule;

public class MySchedulesViewModel
{
    public IEnumerable<MyScheduleViewModel> Schedules { get; set; } = null!;
}

public class MyScheduleViewModel
{
    public int ScheduleId { get; set; }
    public string Time { get; set; } = null!;
    public string Status { get; set; } = null!;
}
