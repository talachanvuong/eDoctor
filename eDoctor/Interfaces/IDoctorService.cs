using eDoctor.Areas.Doctor.Models.Dtos.Doctor.Queries;
using eDoctor.Models.Dtos.Doctor;
using eDoctor.Models.Dtos.Doctor.Fallbacks;
using eDoctor.Models.Dtos.Doctor.Queries;
using eDoctor.Results;

namespace eDoctor.Interfaces;

public interface IDoctorService
{
    Task<Result<DoctorsDto, DoctorsFallbackDto>> GetByDepartmentAsync(DoctorsQueryDto dto);
    Task<Result<AboutDto, AboutFallbackDto>> GetAboutAsync(AboutQueryDto dto);
    Task<Result> LoginAsync(LoginQueryDto dto);
    Task LogoutAsync();
}
