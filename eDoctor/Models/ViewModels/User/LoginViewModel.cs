using eDoctor.Attributes;
using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models.ViewModels.User;

public class LoginViewModel
{
    [Display(Name = "Login name")]
    [Required]
    [LoginName]
    [StringLength(64, MinimumLength = 4)]
    public string LoginName { get; set; } = null!;

    [Display(Name = "Password")]
    [Required]
    [Password]
    [StringLength(32, MinimumLength = 8)]
    public string Password { get; set; } = null!;
}
