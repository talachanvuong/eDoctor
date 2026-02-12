using eDoctor.Areas.Doctor.Models.Dtos.Doctor;
using eDoctor.Areas.Doctor.Models.Dtos.Doctor.Queries;
using eDoctor.Data;
using eDoctor.Helpers;
using eDoctor.Interfaces;
using eDoctor.Models;
using eDoctor.Models.Dtos.Doctor;
using eDoctor.Models.Dtos.Doctor.Fallbacks;
using eDoctor.Models.Dtos.Doctor.Queries;
using eDoctor.Results;
using Microsoft.EntityFrameworkCore;
using AreaIntroductionDto = eDoctor.Areas.Doctor.Models.Dtos.Doctor.IntroductionDto;
using IntroductionDto = eDoctor.Models.Dtos.Doctor.IntroductionDto;

namespace eDoctor.Services;

public class DoctorService : IDoctorService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordService _passwordService;
    private readonly IAuthService _authService;

    public DoctorService(ApplicationDbContext context, IPasswordService passwordService, IAuthService authService)
    {
        _context = context;
        _passwordService = passwordService;
        _authService = authService;
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

        IEnumerable<OtherDto> others = await _context.Doctors
            .Where(d => d.DepartmentId == detail.DepartmentId && d.DoctorId != dto.DoctorId)
            .OrderBy(d => d.DoctorId)
            .Select(d => new OtherDto
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

    public async Task<Result> LoginAsync(LoginQueryDto dto)
    {
        Doctor? doctor = await _context.Doctors.FirstOrDefaultAsync(u => u.LoginName == dto.LoginName);

        if (doctor == null || !_passwordService.Verify(dto.Password, doctor.Password))
        {
            return Result.Failure("Invalid login name or password.");
        }

        await _authService.LoginAsync(doctor.DoctorId, RoleTypes.Doctor);

        return Result.Success();
    }

    public async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
    }

    public async Task<ProfileDto> GetProfileAsync(ProfileQueryDto dto)
    {
        InfoDto info = await _context.Doctors
            .Where(d => d.DoctorId == dto.DoctorId)
            .Select(d => new InfoDto
            {
                FullName = d.FullName,
                BirthDate = d.BirthDate,
                Gender = d.Gender,
                YearsOfExperience = d.YearsOfExperience,
                Avatar = d.Avatar
            })
            .FirstAsync();

        IEnumerable<AreaIntroductionDto> introductions = await _context.Sections
            .OrderBy(s => s.SectionOrder)
            .Select(s => new AreaIntroductionDto
            {
                SectionId = s.SectionId,
                SectionTitle = s.SectionTitle,
                Content = s.Introductions
                    .Where(i => i.DoctorId == dto.DoctorId)
                    .Select(i => i.Content)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return new ProfileDto
        {
            Info = info,
            Introductions = introductions
        };
    }
}
