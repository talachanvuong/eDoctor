namespace eDoctor.Models.Dtos.User.Queries;

public class RegisterQueryDto
{
    public string FullName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public bool Sex { get; set; }
    public string LoginName { get; set; } = null!;
    public string Password { get; set; } = null!;
}
