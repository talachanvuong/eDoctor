namespace eDoctor.Areas.Doctor.Models.Dtos.MedicalRecord.Queries;

public class CreateMedicalRecordQueryDto
{
    public int ScheduleId { get; set; }
    public int DoctorId { get; set; }
    public string Symptom { get; set; } = null!;
    public string Diagnosis { get; set; } = null!;
    public string Advice { get; set; } = null!;
    public IEnumerable<DetailPrescriptionQueryDto> Prescription { get; set; } = null!;
}

public class DetailPrescriptionQueryDto
{
    public string DrugName { get; set; } = null!;
    public int Quantity { get; set; }
    public string? Note { get; set; }
}
