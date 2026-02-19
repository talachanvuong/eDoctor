using eDoctor.Enums;
using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models;

public class Invoice
{
    [Key]
    public int InvoiceId { get; set; }

    public InvoiceStatus Status { get; set; }

    public DateTime ExpiresAt { get; set; }

    public int ScheduleId { get; set; }

    public Schedule Schedule { get; set; } = null!;

    public ICollection<DetailInvoice> DetailInvoices { get; } = [];

    public Payment Payment { get; set; } = null!;
}
