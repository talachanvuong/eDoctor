using eDoctor.Data;
using eDoctor.Interfaces;
using eDoctor.Models;
using eDoctor.Models.Dtos.User;
using Microsoft.EntityFrameworkCore;

namespace eDoctor.Services;

public class UserService : IUserService
{
    private readonly IPasswordService _passwordService;
    private readonly ApplicationDbContext _context;

    public UserService(IPasswordService passwordService, ApplicationDbContext context)
    {
        _passwordService = passwordService;
        _context = context;
    }

    public async Task AddAsync(RegisterDto dto)
    {
        string hashedPassword = _passwordService.Hash(dto.Password);

        User user = new User
        {
            LoginName = dto.LoginName,
            Password = hashedPassword,
            FullName = dto.FullName,
            BirthDate = dto.BirthDate,
            Sex = dto.Sex
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByLoginNameAsync(string loginName)
    {
        return await _context.Users.AnyAsync(u => u.LoginName == loginName);
    }

    public async Task<bool> CheckPasswordAsync(string loginName, string password)
    {
        User? user = await _context.Users.SingleOrDefaultAsync(u => u.LoginName == loginName);

        if (user == null)
        {
            return false;
        }

        return _passwordService.Verify(password, user.Password);
    }
}
