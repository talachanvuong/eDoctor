namespace eDoctor.Models.Dtos.User.Queries;

public class LoginQueryDto
{
    public string LoginName { get; set; } = null!;
    public string Password { get; set; } = null!;
}
