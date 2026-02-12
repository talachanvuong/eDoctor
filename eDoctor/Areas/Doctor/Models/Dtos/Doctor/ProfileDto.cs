namespace eDoctor.Areas.Doctor.Models.Dtos.Doctor;

public class ProfileDto
{
    public InfoDto Info { get; set; } = null!;
    public IEnumerable<IntroductionDto> Introductions { get; set; } = null!;
}

public class InfoDto
{
    public string FullName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public bool Gender { get; set; }
    public int YearsOfExperience { get; set; }
    public byte[] Avatar { get; set; } = null!;
}

public class IntroductionDto
{
    public int SectionId { get; set; }
    public string SectionTitle { get; set; } = null!;
    public string? Content { get; set; }
}
