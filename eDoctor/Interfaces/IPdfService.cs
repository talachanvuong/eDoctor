using eDoctor.Models.Dtos.Payment;

namespace eDoctor.Interfaces;

public interface IPdfService
{
    byte[] GenerateInvoice(DetailInvoiceDto dto);
}
