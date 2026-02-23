namespace eDoctor.Models.ViewModels.Payment;

public class BillViewModel
{
    public int ScheduleId { get; set; }
    public IEnumerable<ServiceViewModel> Services { get; set; } = null!;
    public decimal Total { get; set; }
    public string Note { get; set; } = null!;
}

public class ServiceViewModel
{
    public string ServiceName { get; set; } = null!;
    public decimal Price { get; set; }
}
