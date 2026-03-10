using eDoctor.Enums;
using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Notification;
using eDoctor.Models.Dtos.Notification.Queries;
using eDoctor.Models.ViewModels.Notification;
using eDoctor.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eDoctor.Controllers;

public class NotificationController : Controller
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> GetMyNotifications()
    {
        MyQueryDto dto = new MyQueryDto
        {
            Id = User.GetId(),
            ActorType = ActorType.USER
        };

        MyDto notifications = await _notificationService.GetMyAsync(dto);

        return Ok(new
        {
            data = notifications
        });
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> Read([FromBody] ReadViewModel vm)
    {
        ReadQueryDto dto = new ReadQueryDto
        {
            NotificationId = vm.NotificationId,
            Receiver = User.GetId(),
            ActorType = ActorType.USER
        };

        Result result = await _notificationService.ReadAsync(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(new
            {
                message = result.Error
            });
        }

        return Ok();
    }
}
