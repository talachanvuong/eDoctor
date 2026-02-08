using eDoctor.Models.Dtos.Doctor;

namespace eDoctor.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<BriefDto>> GetByDepartmentAsync(int departmentId);
}
