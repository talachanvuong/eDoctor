using eDoctor.Models.Dtos.User;

namespace eDoctor.Interfaces;

public interface IUserService
{
    Task AddAsync(RegisterDto dto);
    Task<bool> ExistsByLoginNameAsync(string loginName);
    Task<IdDto?> CheckPasswordAsync(string loginName, string password);
    Task<bool> CheckPasswordAsync(int userId, string password);
    Task<ProfileDto> GetProfileAsync(int userId);
    Task UpdateAsync(int userId, UpdateDto dto);
    Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
}
