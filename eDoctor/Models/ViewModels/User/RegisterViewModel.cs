using eDoctor.Attributes;
using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models.ViewModels.User;

public class RegisterViewModel
{
    [Display(Name = "Full name")]
    [Required]
    [StringLength(64, MinimumLength = 4)]
    public string FullName { get; set; } = null!;

    [Display(Name = "Date of birth")]
    [Required]
    [Age(18, 120)]
    public DateTime BirthDate { get; set; }

    public bool Sex { get; set; }

    [Display(Name = "Login name")]
    [Required]
    [LoginName]
    [StringLength(64, MinimumLength = 4)]
    public string LoginName { get; set; } = null!;

    [Required]
    [Password]
    [StringLength(32, MinimumLength = 8)]
    public string Password { get; set; } = null!;

    [Display(Name = "Confirm password")]
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = null!;
}
