using eDoctor.Areas.Doctor.Models.Dtos.MedicalRecord;
using eDoctor.Areas.Doctor.Models.Dtos.MedicalRecord.Queries;
using eDoctor.Areas.Doctor.Models.ViewModels.MedicalRecord;
using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Areas.Doctor.Controllers;

[Area("Doctor")]
public class MedicalRecordController : Controller
{
    private readonly IMedicalRecordService _medicalRecordService;

    public MedicalRecordController(IMedicalRecordService medicalRecordService)
    {
        _medicalRecordService = medicalRecordService;
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.Doctor)]
    public IActionResult CreateMedicalRecord(CreateMedicalRecordViewModel vm)
    {
        ModelState.Clear();
        return View(vm);
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.Doctor)]
    public async Task<IActionResult> Create(CreateMedicalRecordViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("CreateMedicalRecord", vm);
        }

        CreateMedicalRecordQueryDto dto = new CreateMedicalRecordQueryDto
        {
            ScheduleId = vm.ScheduleId,
            DoctorId = User.GetId(),
            Symptom = vm.Symptom,
            Diagnosis = vm.Diagnosis,
            Advice = vm.Advice,
            Prescription = vm.Prescription.Select(d => new DetailPrescriptionQueryDto
            {
                DrugName = d.DrugName,
                Quantity = d.Quantity,
                Note = d.Note
            })
        };

        Result result = await _medicalRecordService.AddAsync(dto);

        if (!result.IsSuccess)
        {
            TempData.SetAlert(result.Error!, AlertTypes.Danger);

            return RedirectToAction("Index", "Home");
        }

        TempData.SetAlert("Create medical record successfully!", AlertTypes.Success);

        return RedirectToAction("DetailSchedule", "Home", new { vm.ScheduleId });
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.Doctor)]
    public async Task<IActionResult> Detail(MedicalRecordViewModel vm)
    {
        MedicalRecordQueryDto dto = new MedicalRecordQueryDto
        {
            ScheduleId = vm.ScheduleId
        };

        Result<MedicalRecordDto> result = await _medicalRecordService.GetDoctorMedicalRecordAsync(dto);

        if (!result.IsSuccess)
        {
            TempData.SetAlert(result.Error!, AlertTypes.Danger);

            return RedirectToAction("Index", "Home");
        }

        MedicalRecordDto medicalRecord = result.Value!;

        return File(medicalRecord.Pdf, "application/pdf");
    }
}
