using eDoctor.Attributes;
using System.ComponentModel.DataAnnotations;

namespace eDoctor.Areas.Doctor.Models.ViewModels.Schedule;

public class CreateViewModel
{
    [Display(Name = "Start time")]
    [Required]
    [MinMinutes(60)]
    [MaxDays(30)]
    public DateTime StartTime { get; set; }
}
