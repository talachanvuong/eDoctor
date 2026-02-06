using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models;

public class Introduction
{
    [Key]
    public int IntroductionId { get; set; }

    [MaxLength(2048)]
    public string Content { get; set; } = null!;

    public int SectionId { get; set; }

    public int DoctorId { get; set; }

    public Section Section { get; set; } = null!;

    public Doctor Doctor { get; set; } = null!;
}
