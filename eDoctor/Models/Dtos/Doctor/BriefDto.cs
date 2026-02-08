namespace eDoctor.Models.Dtos.Doctor;

public class BriefDto
{
    public int DoctorId { get; set; }
    public byte[] Avatar { get; set; } = null!;
    public string FullName { get; set; } = null!;
}
