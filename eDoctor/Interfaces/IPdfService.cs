using eDoctor.Areas.Doctor.Models.Dtos.MedicalRecord;
using eDoctor.Models.Dtos.Payment;

namespace eDoctor.Interfaces;

public interface IPdfService
{
    byte[] GenerateInvoice(DetailInvoiceDto dto);
    byte[] GenerateMedicalRecord(DetailMedicalRecordDto dto);
}
