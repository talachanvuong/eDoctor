using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Meeting.Queries;
using eDoctor.Models.ViewModels.Meeting;
using eDoctor.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Areas.Doctor.Controllers;

[Area("Doctor")]
public class MeetingController : Controller
{
    private readonly IMeetingService _meetingService;

    public MeetingController(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.Doctor)]
    public async Task<IActionResult> Join(RoomViewModel vm)
    {
        RoomQueryDto dto = new RoomQueryDto
        {
            Room = vm.Room,
            Id = User.GetId(),
            Role = User.GetRole()
        };

        Result result = await _meetingService.JoinAsync(dto);

        if (!result.IsSuccess)
        {
            TempData.SetAlert(result.Error!, AlertTypes.Danger);

            return RedirectToAction("Index", "Home");
        }

        return View(vm);
    }
}
