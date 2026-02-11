namespace eDoctor.Models.Dtos.User.Queries;

public class UpdateQueryDto
{
    public int UserId { get; set; }
    public string FullName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public bool Sex { get; set; }
}
