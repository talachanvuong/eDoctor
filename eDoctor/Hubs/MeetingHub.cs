using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Meeting.Queries;
using eDoctor.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace eDoctor.Hubs;

[Authorize]
public class MeetingHub : Hub
{
    private readonly IMeetingService _meetingService;

    private static readonly Dictionary<string, List<RoomPaticipant>> _room = new();

    public MeetingHub(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }

    public async Task Join(string room)
    {
        string role = Context.User!.GetRole();

        RoomQueryDto dto = new RoomQueryDto
        {
            Room = room,
            Id = Context.User!.GetId(),
            Role = Context.User!.GetRole()
        };

        Result result = await _meetingService.JoinAsync(dto);

        if (!result.IsSuccess)
        {
            await Clients.Caller.SendAsync("Error", result.Error);

            return;
        }

        if (!_room.ContainsKey(room))
        {
            _room[room] = new List<RoomPaticipant>();
        }

        if (_room[room].Any(p => p.Role == role))
        {
            await Clients.Caller.SendAsync("Error", "You already joined this room.");

            return;
        }

        _room[room].Add(new RoomPaticipant
        {
            ConnectionId = Context.ConnectionId,
            Role = role
        });

        await Groups.AddToGroupAsync(Context.ConnectionId, room);
        await Clients.OthersInGroup(room).SendAsync("UserJoined");
    }

    public async Task SendSignal(string room, string data)
    {
        await Clients.OthersInGroup(room).SendAsync("ReceiveSignal", data);
    }

    public async Task CameraStateChanged(string room, bool isOn)
    {
        await Clients.OthersInGroup(room).SendAsync("RemoteCameraChanged", isOn);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var dict = _room.FirstOrDefault(r => r.Value.Any(p => p.ConnectionId == Context.ConnectionId));

        if (dict.Key != null)
        {
            await Clients.OthersInGroup(dict.Key).SendAsync("UserLeft");

            dict.Value.RemoveAll(p => p.ConnectionId == Context.ConnectionId);

            if (dict.Value.Count == 0)
            {
                _room.Remove(dict.Key);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}

public class RoomPaticipant
{
    public string ConnectionId { get; set; } = null!;
    public string Role { get; set; } = null!;
}
