namespace eDoctor.Models.Dtos.Payment.Queries;

public class CaptureQueryDto
{
    public int ScheduleId { get; set; }
    public int UserId { get; set; }
    public string OrderId { get; set; } = null!;
    public IEnumerable<ServiceQueryDto> Services { get; set; } = null!;
}

public class ServiceQueryDto
{
    public string ServiceName { get; set; } = null!;
    public decimal Price { get; set; }
}
