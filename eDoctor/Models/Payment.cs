using eDoctor.Enums;
using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models;

public class Payment
{
    [Key]
    public int PaymentId { get; set; }

    [MaxLength(64)]
    public string Gateway { get; set; } = null!;

    [MaxLength(256)]
    public string GatewayOrderId { get; set; } = null!;

    public PaymentStatus Status { get; set; }

    public int InvoiceId { get; set; }

    public Invoice Invoice { get; set; } = null!;
}
