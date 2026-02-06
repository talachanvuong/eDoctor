using eDoctor.Models;
using eDoctor.Models.Dtos.User;

namespace eDoctor.Interfaces;

public interface IUserService
{
    Task AddAsync(RegisterDto dto);
    Task<bool> ExistsByLoginNameAsync(string loginName);
    Task<User?> CheckPasswordAsync(string loginName, string password);
    Task<bool> CheckPasswordAsync(int userId, string password);
    Task<User> GetCurrentAsync(int userId);
    Task UpdateAsync(int userId, UpdateDto dto);
    Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
}
