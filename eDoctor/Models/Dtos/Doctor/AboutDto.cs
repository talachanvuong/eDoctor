using eDoctor.Enums;

namespace eDoctor.Models.Dtos.Doctor;

public class AboutDto
{
    public DetailDto Detail { get; set; } = null!;
    public IEnumerable<BriefDto> Others { get; set; } = null!;
}

public class DetailDto
{
    public int DoctorId { get; set; }
    public string FullName { get; set; } = null!;
    public RankCode RankCode { get; set; }
    public int YearsOfExperience { get; set; }
    public byte[] Avatar { get; set; } = null!;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = null!;
    public IEnumerable<IntroductionDto> Introductions { get; set; } = null!;
}

public class IntroductionDto
{
    public string SectionTitle { get; set; } = null!;
    public string Content { get; set; } = null!;
}
