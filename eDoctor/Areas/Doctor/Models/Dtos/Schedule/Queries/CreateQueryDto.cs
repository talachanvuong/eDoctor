namespace eDoctor.Areas.Doctor.Models.Dtos.Schedule.Queries;

public class CreateQueryDto
{
    public int DoctorId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
