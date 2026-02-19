using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models;

public class MedicalRecord
{
    [Key]
    public int MedicalRecordId { get; set; }

    [MaxLength(256)]
    public string Symptom { get; set; } = null!;

    [MaxLength(256)]
    public string Diagnosis { get; set; } = null!;

    [MaxLength(256)]
    public string Advice { get; set; } = null!;

    public int ScheduleId { get; set; }

    public Schedule Schedule { get; set; } = null!;

    public ICollection<DetailPrescription> DetailPrescriptions { get; } = [];
}
