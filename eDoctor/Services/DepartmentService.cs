using eDoctor.Data;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Department;
using Microsoft.EntityFrameworkCore;

namespace eDoctor.Services;

public class DepartmentService : IDepartmentService
{
    private readonly ApplicationDbContext _context;

    public DepartmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        return await _context.Departments
            .OrderBy(d => d.DepartmentId)
            .Select(d => new DepartmentDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName
            })
            .AsNoTracking()
            .ToListAsync();
    }
}
