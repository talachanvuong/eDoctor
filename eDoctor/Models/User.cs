using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [MaxLength(64)]
    public string LoginName { get; set; } = null!;

    [MaxLength(256)]
    public string Password { get; set; } = null!;

    [MaxLength(64)]
    public string FullName { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public bool Sex { get; set; }

    public ICollection<Schedule> Schedules { get; } = [];
}
