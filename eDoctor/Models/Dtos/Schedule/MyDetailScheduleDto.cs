using eDoctor.Enums;

namespace eDoctor.Models.Dtos.Schedule;

public class MyDetailScheduleDto
{
    public string Room { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ScheduleStatus Status { get; set; }
    public RankCode RankCode { get; set; }
    public string FullName { get; set; } = null!;
}
