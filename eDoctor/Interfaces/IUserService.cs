using eDoctor.Models.Dtos.User;
using eDoctor.Models.Dtos.User.Queries;
using eDoctor.Results;

namespace eDoctor.Interfaces;

public interface IUserService
{
    Task<Result> AddAsync(RegisterQueryDto dto);
    Task<ProfileDto> GetProfileAsync(ProfileQueryDto dto);
    Task UpdateAsync(UpdateQueryDto dto);
    Task<Result> ChangePasswordAsync(ChangePasswordQueryDto dto);
    Task<Result> LoginAsync(LoginQueryDto dto);
    Task LogoutAsync();
}
