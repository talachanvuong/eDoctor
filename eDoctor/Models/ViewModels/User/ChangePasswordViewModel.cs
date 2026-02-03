using eDoctor.Attributes;
using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models.ViewModels.User;

public class ChangePasswordViewModel
{
    [Display(Name = "Old password")]
    [Required]
    [Password]
    [StringLength(32, MinimumLength = 8)]
    public string OldPassword { get; set; } = null!;

    [Display(Name = "New password")]
    [Required]
    [Password]
    [StringLength(32, MinimumLength = 8)]
    public string NewPassword { get; set; } = null!;

    [Display(Name = "Confirm password")]
    [Required]
    [Compare("NewPassword")]
    public string ConfirmPassword { get; set; } = null!;
}
