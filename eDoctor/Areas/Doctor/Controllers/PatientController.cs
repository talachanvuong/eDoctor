using eDoctor.Areas.Doctor.Models.Dtos.User;
using eDoctor.Areas.Doctor.Models.Dtos.User.Queries;
using eDoctor.Areas.Doctor.Models.ViewModels.User;
using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Areas.Doctor.Controllers;

[Area("Doctor")]
public class PatientController : Controller
{
    private readonly IScheduleService _scheduleService;

    public PatientController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.Doctor)]
    public async Task<IActionResult> History(PatientHistoriesViewModel vm)
    {
        PatientHistoriesQueryDto dto = new PatientHistoriesQueryDto
        {
            UserId = vm.UserId
        };

        Result<PatientHistoriesDto> result = await _scheduleService.GetPatientHistoriesAsync(dto);

        if (!result.IsSuccess)
        {
            TempData.SetAlert(result.Error!, AlertTypes.Danger);

            return RedirectToAction("Index", "Home");
        }

        PatientHistoriesDto histories = result.Value!;

        vm.PatientHistories = histories.PatientHistories.Select(h => new PatientHistoryViewModel
        {
            ScheduleId = h.ScheduleId,
            Time = DateTimeHelper.ConvertToString(h.StartTime, h.EndTime),
            Doctor = $"{h.RankCode.ConvertToString()} {h.FullName}",
            HasMedicalRecord = h.HasMedicalRecord
        });

        return View(vm);
    }
}
