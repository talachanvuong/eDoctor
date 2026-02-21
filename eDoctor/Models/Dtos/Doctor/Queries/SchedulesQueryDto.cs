namespace eDoctor.Models.Dtos.Doctor.Queries;

public class SchedulesQueryDto
{
    public int DoctorId { get; set; }
    public DateTime? Date { get; set; }
    public int UserId { get; set; }
}
