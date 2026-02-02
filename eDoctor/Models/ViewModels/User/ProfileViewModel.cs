using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models.ViewModels.User;

public class ProfileViewModel
{
    [Display(Name = "Full name")]
    [Required]
    [StringLength(64, MinimumLength = 4)]
    public string FullName { get; set; } = null!;

    [Display(Name = "Date of birth")]
    public string BirthDate { get; set; } = null!;

    [Display(Name = "Sex")]
    public string Sex { get; set; } = null!;
}
