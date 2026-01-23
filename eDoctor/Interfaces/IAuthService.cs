namespace eDoctor.Interfaces;

public interface IAuthService
{
    Task LoginAsync(string loginName, string role);
    Task LogoutAsync();
}
