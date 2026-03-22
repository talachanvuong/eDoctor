using eDoctor.Helpers;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Meeting.Queries;
using eDoctor.Models.ViewModels.Meeting;
using eDoctor.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace eDoctor.Controllers;

public class MeetingController : Controller
{
    private readonly IMeetingService _meetingService;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public MeetingController(IMeetingService meetingService, IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _meetingService = meetingService;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
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

        return View(vm);
    }

    [HttpGet]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> GetTurn()
    {
        var client = _httpClientFactory.CreateClient();

        var url = $"https://e-doctor.metered.live/api/v1/turn/credential?secretKey={_configuration["Metered:SecretKey"]}";

        var response = await client.PostAsJsonAsync(url, new
        {
            expiryInSeconds = 3600
        });

        var content = await response.Content.ReadFromJsonAsync<JsonElement>();

        return Ok(new
        {
            Username = content.GetProperty("username").GetString(),
            Password = content.GetProperty("password").GetString()
        });
    }
}
