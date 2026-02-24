using eDoctor.Enums;

namespace eDoctor.Models.Dtos.Schedule;

public class MySchedulesDto
{
    public IEnumerable<MyScheduleDto> Schedules { get; set; } = null!;
}

public class MyScheduleDto
{
    public int ScheduleId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ScheduleStatus Status { get; set; }
}
