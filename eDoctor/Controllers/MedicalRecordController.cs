using eDoctor.Areas.Doctor.Models.Dtos.MedicalRecord;
using eDoctor.Areas.Doctor.Models.Dtos.MedicalRecord.Queries;
using eDoctor.Areas.Doctor.Models.ViewModels.MedicalRecord;
using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Controllers;

public class MedicalRecordController : Controller
{
    private readonly IMedicalRecordService _medicalRecordService;

    public MedicalRecordController(IMedicalRecordService medicalRecordService)
    {
        _medicalRecordService = medicalRecordService;
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> Detail(MedicalRecordViewModel vm)
    {
        MedicalRecordQueryDto dto = new MedicalRecordQueryDto
        {
            ScheduleId = vm.ScheduleId,
            Id = User.GetId()
        };

        Result<MedicalRecordDto> result = await _medicalRecordService.GetUserMedicalRecordAsync(dto);

        if (!result.IsSuccess)
        {
            TempData.SetAlert(result.Error!, AlertTypes.Danger);

            return RedirectToAction("Index", "Home");
        }

        MedicalRecordDto medicalRecord = result.Value!;

        return File(medicalRecord.Pdf, "application/pdf");
    }
}
