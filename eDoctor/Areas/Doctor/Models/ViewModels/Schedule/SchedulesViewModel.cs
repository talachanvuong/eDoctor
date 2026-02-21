using eDoctor.Enums;

namespace eDoctor.Areas.Doctor.Models.ViewModels.Schedule;

public class SchedulesViewModel
{
    public DateTime? Date { get; set; }
    public ScheduleStatus? Status { get; set; }
    public IEnumerable<ScheduleViewModel> Schedules { get; set; } = null!;
}

public class ScheduleViewModel
{
    public int ScheduleId { get; set; }
    public string Time { get; set; } = null!;
    public string Status { get; set; } = null!;
}
