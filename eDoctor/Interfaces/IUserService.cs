using eDoctor.Models.Dtos.User;

namespace eDoctor.Interfaces;

public interface IUserService
{
    Task AddAsync(RegisterDto dto);
    Task<bool> ExistsByLoginNameAsync(string loginName);
    Task<bool> CheckPasswordAsync(string loginName, string password);
}
