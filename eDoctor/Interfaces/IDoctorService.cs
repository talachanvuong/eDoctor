using eDoctor.Models.Dtos.Doctor;
using eDoctor.Models.Dtos.Doctor.Fallbacks;
using eDoctor.Models.Dtos.Doctor.Queries;
using eDoctor.Results;

namespace eDoctor.Interfaces;

public interface IDoctorService
{
    Task<Result<DoctorsDto, DoctorsFallbackDto>> GetByDepartmentAsync(DoctorsQueryDto dto);
}
