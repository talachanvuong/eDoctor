using eDoctor.Enums;

namespace eDoctor.Areas.Doctor.Models.Dtos.User;

public class PatientHistoriesDto
{
    public IEnumerable<PatientHistoryDto> PatientHistories { get; set; } = null!;
}

public class PatientHistoryDto
{
    public int ScheduleId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public RankCode RankCode { get; set; }
    public string FullName { get; set; } = null!;
}
