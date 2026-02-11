using eDoctor.Attributes;
using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models.ViewModels.User;

public class ProfileViewModel
{
    [Display(Name = "Full name")]
    [Required]
    [StringLength(64, MinimumLength = 4)]
    public string FullName { get; set; } = null!;

    [Display(Name = "Date of birth")]
    [Required]
    [Age(18, 120)]
    public DateTime BirthDate { get; set; }

    [Display(Name = "Sex")]
    [Required]
    public bool Sex { get; set; }
}
