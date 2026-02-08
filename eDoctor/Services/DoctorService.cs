using eDoctor.Data;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Doctor;
using Microsoft.EntityFrameworkCore;

namespace eDoctor.Services;

public class DoctorService : IDoctorService
{
    private readonly ApplicationDbContext _context;

    public DoctorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BriefDto>> GetByDepartmentAsync(int departmentId)
    {
        return await _context.Doctors
            .Where(d => d.DepartmentId == departmentId)
            .OrderBy(d => d.DoctorId)
            .Select(d => new BriefDto
            {
                DoctorId = d.DoctorId,
                Avatar = d.Avatar,
                FullName = d.FullName
            })
            .AsNoTracking()
            .ToListAsync();
    }
}
