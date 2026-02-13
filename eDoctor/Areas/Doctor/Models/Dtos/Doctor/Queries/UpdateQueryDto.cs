namespace eDoctor.Areas.Doctor.Models.Dtos.Doctor.Queries;

public class UpdateQueryDto
{
    public int DoctorId { get; set; }
    public string FullName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public bool Gender { get; set; }
    public int YearsOfExperience { get; set; }
    public byte[]? Avatar { get; set; }
    public IEnumerable<IntroductionQueryDto> Introductions { get; set; } = null!;
}

public class IntroductionQueryDto
{
    public int SectionId { get; set; }
    public string Content { get; set; } = null!;
}
