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

    public async Task<Result<AboutDto, AboutFallbackDto>> GetAboutAsync(AboutQueryDto dto)
    {
        if (!await _context.Doctors.AnyAsync(d => d.DoctorId == dto.DoctorId))
        {
            Doctor firstDoctor = await _context.Doctors
                .OrderBy(d => d.DoctorId)
                .FirstAsync();

            AboutFallbackDto fallback = new AboutFallbackDto
            {
                DoctorId = firstDoctor.DoctorId
            };

            return Result<AboutDto, AboutFallbackDto>.Failure("Doctor not found.", fallback);
        }

        DetailDto detail = await _context.Doctors
            .Where(d => d.DoctorId == dto.DoctorId)
            .Select(d => new DetailDto
            {
                DoctorId = d.DoctorId,
                FullName = d.FullName,
                RankCode = d.RankCode,
                YearsOfExperience = d.YearsOfExperience,
                Avatar = d.Avatar,
                DepartmentId = d.DepartmentId,
                DepartmentName = d.Department.DepartmentName,
                Introductions = d.Introductions
                    .OrderBy(i => i.Section.SectionOrder)
                    .Select(i => new IntroductionDto
                    {
                        SectionTitle = i.Section.SectionTitle,
                        Content = i.Content
                    })
                    .ToList()
            })
            .FirstAsync();

        IEnumerable<BriefDto> others = await _context.Doctors
            .Where(d => d.DepartmentId == detail.DepartmentId && d.DoctorId != detail.DoctorId)
            .OrderBy(d => d.DoctorId)
            .Select(d => new BriefDto
            {
                DoctorId = d.DoctorId,
                Avatar = d.Avatar,
                FullName = d.FullName
            })
            .ToListAsync();

        AboutDto value = new AboutDto
        {
            Detail = detail,
            Others = others
        };

        return Result<AboutDto, AboutFallbackDto>.Success(value);
    }
}
