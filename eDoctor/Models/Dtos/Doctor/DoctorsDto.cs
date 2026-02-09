namespace eDoctor.Models.Dtos.Doctor;

public class DoctorsDto
{
    public IEnumerable<DepartmentDto> Departments { get; set; } = null!;
    public IEnumerable<BriefDto> Doctors { get; set; } = null!;
}

public class DepartmentDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = null!;
}

public class BriefDto
{
    public int DoctorId { get; set; }
    public byte[] Avatar { get; set; } = null!;
    public string FullName { get; set; } = null!;
}
