using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models;

public class Service
{
    [Key]
    public int ServiceId { get; set; }

    [MaxLength(256)]
    public string ServiceName { get; set; } = null!;

    public decimal Price { get; set; }
}
