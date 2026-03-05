namespace eDoctor.Areas.Doctor.Models.ViewModels.User;

public class PatientHistoriesViewModel
{
    public int UserId { get; set; }
    public IEnumerable<PatientHistoryViewModel> PatientHistories { get; set; } = null!;
}

public class PatientHistoryViewModel
{
    public int ScheduleId { get; set; }
    public string Time { get; set; } = null!;
    public string Doctor { get; set; } = null!;
}
