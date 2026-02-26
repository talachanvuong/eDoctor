namespace eDoctor.Models.Dtos.Payment;

public class InvoiceDto
{
    public byte[] Pdf { get; set; } = null!;
}

public class DetailInvoiceDto
{
    public int InvoiceId { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<ServiceDto> Services { get; set; } = null!;
    public string Payer { get; set; } = null!;
}
