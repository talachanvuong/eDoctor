using eDoctor.Enums;

namespace eDoctor.Models.ViewModels.Doctor;

public class AboutViewModel
{
    public int DoctorId { get; set; }
    public DetailViewModel Detail { get; set; } = null!;
    public IEnumerable<BriefViewModel> Others { get; set; } = null!;
}

public class DetailViewModel
{
    public int DoctorId { get; set; }
    public string FullName { get; set; } = null!;
    public RankCode RankCode { get; set; }
    public int YearsOfExperience { get; set; }
    public string Avatar { get; set; } = null!;
    public string DepartmentName { get; set; } = null!;
    public IEnumerable<IntroductionViewModel> Introductions { get; set; } = null!;
}

public class IntroductionViewModel
{
    public string SectionTitle { get; set; } = null!;
    public string Content { get; set; } = null!;
}
