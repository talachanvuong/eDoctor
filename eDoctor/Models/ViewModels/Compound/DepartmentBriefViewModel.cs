using eDoctor.Models.ViewModels.Department;
using eDoctor.Models.ViewModels.Doctor;

namespace eDoctor.Models.ViewModels.Compound;

public class DepartmentBriefViewModel
{
    public int DepartmentId { get; set; } = 1;
    public IEnumerable<DepartmentViewModel> Departments { get; set; } = null!;
    public IEnumerable<BriefViewModel> Doctors { get; set; } = null!;
}
