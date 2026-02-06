using System.ComponentModel.DataAnnotations;

namespace eDoctor.Enums;

public enum RankCode
{
    [Display(Name = "Dr.")]
    Doctor,

    [Display(Name = "MS")]
    MasterOfScience,

    [Display(Name = "PhD")]
    DoctorOfPhilosophy,

    [Display(Name = "Prof.")]
    Professor
}
