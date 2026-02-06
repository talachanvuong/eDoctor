using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models;

public class Section
{
    [Key]
    public int SectionId { get; set; }

    [MaxLength(64)]
    public string SectionTitle { get; set; } = null!;

    public int SectionOrder { get; set; }

    public ICollection<Introduction> Introductions { get; } = [];
}
