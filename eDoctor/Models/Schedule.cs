using eDoctor.Enums;
using System.ComponentModel.DataAnnotations;

namespace eDoctor.Models;

public class Schedule
{
    [Key]
    public int ScheduleId { get; set; }

    [MaxLength(256)]
    public string? Room { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public ScheduleStatus Status { get; set; }

    public int? UserId { get; set; }

    public int DoctorId { get; set; }

    public User? User { get; set; }

    public Doctor Doctor { get; set; } = null!;

    public MedicalRecord? MedicalRecord { get; set; }

    public Invoice? Invoice { get; set; }
}
