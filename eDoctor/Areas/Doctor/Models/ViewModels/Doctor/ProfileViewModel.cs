using eDoctor.Attributes;
using eDoctor.Enums;
using System.ComponentModel.DataAnnotations;

namespace eDoctor.Areas.Doctor.Models.ViewModels.Doctor;

public class ProfileViewModel
{
    public InfoViewModel Info { get; set; } = null!;
    public List<IntroductionViewModel> Introductions { get; set; } = null!;
}

public class InfoViewModel
{
    [Display(Name = "Full name")]
    [Required]
    [StringLength(64, MinimumLength = 4)]
    public string FullName { get; set; } = null!;

    [Display(Name = "Date of birth")]
    [Required]
    [Age(18, 120)]
    public DateTime BirthDate { get; set; }

    public bool Gender { get; set; }

    [Display(Name = "Academic rank")]
    public string RankCode { get; set; } = null!;

    [Display(Name = "Experience")]
    [Required]
    [Range(0, 60)]
    public int YearsOfExperience { get; set; }

    public string Avatar { get; set; } = null!;

    [Display(Name = "Avatar")]
    [Image(2 * 1024 * 1024, [".png"])]
    public IFormFile? AvatarFile { get; set; }

    [Display(Name = "Department")]
    public string DepartmentName { get; set; } = null!;
}

public class IntroductionViewModel
{
    public int SectionId { get; set; }

    public string SectionTitle { get; set; } = null!;

    [StringLength(2048)]
    public string? Content { get; set; }
}
