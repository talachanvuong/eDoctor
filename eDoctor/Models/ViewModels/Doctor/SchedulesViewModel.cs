namespace eDoctor.Models.ViewModels.Doctor;

public class SchedulesViewModel
{
    public int DoctorId { get; set; }
    public DateTime? Date { get; set; }
    public IEnumerable<ScheduleViewModel> Schedules { get; set; } = null!;
}

public class ScheduleViewModel
{
    public int ScheduleId { get; set; }
    public string Time { get; set; } = null!;
}
