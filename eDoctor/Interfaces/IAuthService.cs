namespace eDoctor.Interfaces;

public interface IAuthService
{
    Task LoginAsync(int id, string role);
    Task LogoutAsync();
}
