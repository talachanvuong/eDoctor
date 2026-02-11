using eDoctor.Data;
using eDoctor.Helpers;
using eDoctor.Interfaces;
using eDoctor.Models;
using eDoctor.Models.Dtos.User;
using eDoctor.Models.Dtos.User.Queries;
using eDoctor.Results;
using Microsoft.EntityFrameworkCore;

namespace eDoctor.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordService _passwordService;
    private readonly IAuthService _authService;

    public UserService(ApplicationDbContext context, IPasswordService passwordService, IAuthService authService)
    {
        _context = context;
        _passwordService = passwordService;
        _authService = authService;
    }

    public async Task<Result> AddAsync(RegisterQueryDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.LoginName == dto.LoginName))
        {
            return Result.Failure("User already exists.");
        }

        User user = new User
        {
            LoginName = dto.LoginName,
            Password = _passwordService.Hash(dto.Password),
            FullName = dto.FullName,
            BirthDate = dto.BirthDate,
            Sex = dto.Sex
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<ProfileDto> GetProfileAsync(ProfileQueryDto dto)
    {
        return await _context.Users
            .Where(u => u.UserId == dto.UserId)
            .Select(u => new ProfileDto
            {
                FullName = u.FullName,
                BirthDate = u.BirthDate,
                Sex = u.Sex
            })
            .FirstAsync();
    }

    public async Task UpdateAsync(UpdateQueryDto dto)
    {
        User user = await _context.Users.FirstAsync(u => u.UserId == dto.UserId);

        user.FullName = dto.FullName;
        user.BirthDate = dto.BirthDate;
        user.Sex = dto.Sex;

        await _context.SaveChangesAsync();
    }

    public async Task<Result> ChangePasswordAsync(ChangePasswordQueryDto dto)
    {
        User user = await _context.Users.FirstAsync(u => u.UserId == dto.UserId);

        if (!_passwordService.Verify(dto.OldPassword, user.Password))
        {
            return Result.Failure("Wrong old password.");
        }

        user.Password = _passwordService.Hash(dto.NewPassword);

        await _context.SaveChangesAsync();
        await _authService.LogoutAsync();

        return Result.Success();
    }

    public async Task<Result> LoginAsync(LoginQueryDto dto)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.LoginName == dto.LoginName);

        if (user == null || !_passwordService.Verify(dto.Password, user.Password))
        {
            return Result.Failure("Invalid login name or password.");
        }

        await _authService.LoginAsync(user.UserId, RoleTypes.User);

        return Result.Success();
    }

    public async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
    }
}
