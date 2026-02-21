using eDoctor.Enums;

namespace eDoctor.Areas.Doctor.Models.Dtos.Schedule.Queries;

public class SchedulesQueryDto
{
    public DateTime? Date { get; set; }
    public ScheduleStatus? Status { get; set; }
}
