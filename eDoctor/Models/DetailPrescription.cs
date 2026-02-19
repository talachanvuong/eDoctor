using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models;

public class DetailPrescription
{
    [Key]
    public int DetailPrescriptionId { get; set; }

    [MaxLength(256)]
    public string DrugName { get; set; } = null!;

    public int Quantity { get; set; }

    [MaxLength(2048)]
    public string? Note { get; set; }

    public int MedicalRecordId { get; set; }

    public MedicalRecord MedicalRecord { get; set; } = null!;
}
