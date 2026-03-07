using System.ComponentModel.DataAnnotations;

namespace eDoctor.Areas.Doctor.Models.ViewModels.MedicalRecord;

public class CreateMedicalRecordViewModel
{
    public int ScheduleId { get; set; }

    [Required]
    [StringLength(256)]
    public string Symptom { get; set; } = null!;

    [Required]
    [StringLength(256)]
    public string Diagnosis { get; set; } = null!;

    [Required]
    [StringLength(256)]
    public string Advice { get; set; } = null!;

    [MinLength(1)]
    public List<DetailPrescriptionViewModel> Prescription { get; set; } = null!;
}

public class DetailPrescriptionViewModel
{
    [Display(Name = "Drug name")]
    [Required]
    [StringLength(256)]
    public string DrugName { get; set; } = null!;

    [Required]
    [Range(1, 100)]
    public int Quantity { get; set; }

    [StringLength(2048)]
    public string? Note { get; set; }
}
