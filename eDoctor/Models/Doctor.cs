using eDoctor.Enums;
using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models;

public class Doctor
{
    [Key]
    public int DoctorId { get; set; }

    [MaxLength(64)]
    public string LoginName { get; set; } = null!;

    [MaxLength(256)]
    public string Password { get; set; } = null!;

    [MaxLength(64)]
    public string FullName { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public bool Gender { get; set; }

    public RankCode RankCode { get; set; }

    public int YearsOfExperience { get; set; }

    public byte[] Avatar { get; set; } = null!;

    public int DepartmentId { get; set; }

    public ICollection<Introduction> Introductions { get; } = [];

    public Department Department { get; set; } = null!;
}
