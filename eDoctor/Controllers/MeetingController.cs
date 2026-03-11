using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Meeting.Queries;
using eDoctor.Models.ViewModels.Meeting;
using eDoctor.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Controllers;

public class MeetingController : Controller
{
    private readonly IMeetingService _meetingService;
    private readonly IConfiguration _configuration;

    public MeetingController(IMeetingService meetingService, IConfiguration configuration)
    {
        _meetingService = meetingService;
        _configuration = configuration;
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.User)]
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

        ViewBag.Username = _configuration["Metered:Username"];
        ViewBag.Password = _configuration["Metered:Password"];

        return View(vm);
    }
}
