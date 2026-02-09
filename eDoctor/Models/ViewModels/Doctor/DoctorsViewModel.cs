namespace eDoctor.Models.ViewModels.Doctor;

public class DoctorsViewModel
{
    public int DepartmentId { get; set; }
    public IEnumerable<DepartmentViewModel> Departments { get; set; } = null!;
    public IEnumerable<BriefViewModel> Doctors { get; set; } = null!;
}

public class DepartmentViewModel
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = null!;
}

public class BriefViewModel
{
    public int DoctorId { get; set; }
    public string Avatar { get; set; } = null!;
    public string FullName { get; set; } = null!;
}
