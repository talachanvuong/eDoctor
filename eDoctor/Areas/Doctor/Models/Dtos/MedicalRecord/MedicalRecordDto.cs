using eDoctor.Enums;

namespace eDoctor.Areas.Doctor.Models.Dtos.MedicalRecord;

public class MedicalRecordDto
{
    public byte[] Pdf { get; set; } = null!;
}

public class DetailMedicalRecordDto
{
    public int MedicalRecordId { get; set; }
    public string Symptom { get; set; } = null!;
    public string Diagnosis { get; set; } = null!;
    public string Advice { get; set; } = null!;
    public IEnumerable<DetailPrescriptionDto> Prescription { get; set; } = null!;
    public string PatientFullName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public bool Sex { get; set; }
    public string DoctorFullName { get; set; } = null!;
    public RankCode RankCode { get; set; }
}

public class DetailPrescriptionDto
{
    public string DrugName { get; set; } = null!;
    public int Quantity { get; set; }
    public string? Note { get; set; }
}
