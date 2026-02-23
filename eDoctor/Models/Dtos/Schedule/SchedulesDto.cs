namespace eDoctor.Models.Dtos.Schedule;

public class SchedulesDto
{
    public IEnumerable<ScheduleDto> Schedules { get; set; } = null!;
}

public class ScheduleDto
{
    public int ScheduleId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
