using eDoctor.Data;
using eDoctor.Interfaces;
using eDoctor.Models;
using eDoctor.Models.Dtos.Doctor;
using eDoctor.Models.Dtos.Doctor.Fallbacks;
using eDoctor.Models.Dtos.Doctor.Queries;
using eDoctor.Results;
using Microsoft.EntityFrameworkCore;

namespace eDoctor.Services;

public class DoctorService : IDoctorService
{
    private readonly ApplicationDbContext _context;

    public DoctorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DoctorsDto, DoctorsFallbackDto>> GetByDepartmentAsync(DoctorsQueryDto dto)
    {
        if (!await _context.Departments.AnyAsync(d => d.DepartmentId == dto.DepartmentId))
        {
            Department firstDepartment = await _context.Departments
                .OrderBy(d => d.DepartmentId)
                .FirstAsync();

            DoctorsFallbackDto fallback = new DoctorsFallbackDto
            {
                DepartmentId = firstDepartment.DepartmentId
            };

            return Result<DoctorsDto, DoctorsFallbackDto>.Failure("Department not found.", fallback);
        }

        IEnumerable<DepartmentDto> departments = await _context.Departments
            .OrderBy(d => d.DepartmentId)
            .Select(d => new DepartmentDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName
            })
            .ToListAsync();

        IEnumerable<BriefDto> doctors = await _context.Doctors
            .Where(d => d.DepartmentId == dto.DepartmentId)
            .OrderBy(d => d.DoctorId)
            .Select(d => new BriefDto
            {
                DoctorId = d.DoctorId,
                Avatar = d.Avatar,
                FullName = d.FullName
            })
            .ToListAsync();

        DoctorsDto value = new DoctorsDto
        {
            Departments = departments,
            Doctors = doctors
        };

        return Result<DoctorsDto, DoctorsFallbackDto>.Success(value);
    }
}
