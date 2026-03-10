using eDoctor.Helpers.ExtensionMethods;
using Microsoft.AspNetCore.SignalR;

namespace eDoctor.Hubs;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        int id = Context.User!.GetId();
        string role = Context.User!.GetRole();

        await Groups.AddToGroupAsync(Context.ConnectionId, $"{role}-{id}");

        await base.OnConnectedAsync();
    }
}
