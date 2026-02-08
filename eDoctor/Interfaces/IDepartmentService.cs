using eDoctor.Models.Dtos.Department;

namespace eDoctor.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
}
