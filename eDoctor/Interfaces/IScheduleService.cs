using eDoctor.Areas.Doctor.Models.Dtos.Schedule;
using eDoctor.Areas.Doctor.Models.Dtos.Schedule.Queries;
using eDoctor.Results;

namespace eDoctor.Interfaces;

public interface IScheduleService
{
    Task<Result> AddAsync(CreateQueryDto dto);
    Task<SchedulesDto> GetSchedulesAsync(SchedulesQueryDto dto);
    Task<Result<DetailDto>> GetDetailAsync(DetailQueryDto dto);
}
