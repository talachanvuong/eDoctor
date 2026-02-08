namespace eDoctor.Models.Dtos.User;

public class ProfileDto
{
    public string FullName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public bool Sex { get; set; }
}
