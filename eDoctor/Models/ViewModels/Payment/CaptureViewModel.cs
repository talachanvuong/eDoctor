namespace eDoctor.Models.ViewModels.Payment;

public class CaptureViewModel
{
    public int ScheduleId { get; set; }
    public string OrderId { get; set; } = null!;
    public IEnumerable<ServiceViewModel> Services { get; set; } = null!;
}
