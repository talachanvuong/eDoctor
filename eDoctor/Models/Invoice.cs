using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models;

public class Invoice
{
    [Key]
    public int InvoiceId { get; set; }

    [MaxLength(256)]
    public string OrderId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int ScheduleId { get; set; }

    public Schedule Schedule { get; set; } = null!;

    public ICollection<DetailInvoice> DetailInvoices { get; } = [];
}
