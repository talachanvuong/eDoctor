using eDoctor.Enums;

namespace eDoctor.Models.Dtos.Payment;

public class BillDto
{
    public IEnumerable<ServiceDto> Services { get; set; } = null!;
    public RankCode RankCode { get; set; }
    public string FullName { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public class ServiceDto
{
    public string ServiceName { get; set; } = null!;
    public decimal Price { get; set; }
}
