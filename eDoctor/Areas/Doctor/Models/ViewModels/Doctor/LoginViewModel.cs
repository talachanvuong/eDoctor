using eDoctor.Attributes;
using System.ComponentModel.DataAnnotations;

namespace eDoctor.Areas.Doctor.Models.ViewModels.Doctor;

public class LoginViewModel
{
    [Display(Name = "Login name")]
    [Required]
    [LoginName]
    [StringLength(64, MinimumLength = 4)]
    public string LoginName { get; set; } = null!;

    [Required]
    [Password]
    [StringLength(32, MinimumLength = 8)]
    public string Password { get; set; } = null!;
}
