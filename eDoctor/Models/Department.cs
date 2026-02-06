using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models;

public class Department
{
    [Key]
    public int DepartmentId { get; set; }

    [MaxLength(64)]
    public string DepartmentName { get; set; } = null!;

    public ICollection<Doctor> Doctors { get; } = [];
}
