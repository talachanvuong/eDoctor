using eDoctor.Areas.Doctor.Models.Dtos.Schedule;
using eDoctor.Areas.Doctor.Models.Dtos.Schedule.Queries;
using eDoctor.Results;
using AreaSchedulesDto = eDoctor.Areas.Doctor.Models.Dtos.Schedule.SchedulesDto;
using AreaSchedulesQueryDto = eDoctor.Areas.Doctor.Models.Dtos.Schedule.Queries.SchedulesQueryDto;
using SchedulesDto = eDoctor.Models.Dtos.Doctor.SchedulesDto;
using SchedulesQueryDto = eDoctor.Models.Dtos.Doctor.Queries.SchedulesQueryDto;

namespace eDoctor.Interfaces;

public interface IScheduleService
{
    Task<Result> AddAsync(CreateQueryDto dto);
    Task<AreaSchedulesDto> GetSchedulesAsync(AreaSchedulesQueryDto dto);
    Task<Result<DetailDto>> GetDetailAsync(DetailQueryDto dto);
    Task<Result> CancelAsync(CancelQueryDto dto);
    Task<Result<SchedulesDto>> GetSchedulesAsync(SchedulesQueryDto dto);
}
