using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models;

public class DetailInvoice
{
    [Key]
    public int DetailInvoiceId { get; set; }

    [MaxLength(256)]
    public string ServiceName { get; set; } = null!;

    public decimal Price { get; set; }

    public int InvoiceId { get; set; }

    public Invoice Invoice { get; set; } = null!;
}
