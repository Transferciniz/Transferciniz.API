using MediatR;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using Transferciniz.API.Commands.UserLocationCommands;

namespace Transferciniz.API.Hubs;

[SignalRHub]
public class LocationHub: Hub
{
    private readonly IMediator _mediator;
    private static readonly Dictionary<string, int> _groupCounts = new();

    
    public enum SocketMethods
    {
        OnLocationChanged = 0,
        OnVehicleLocationChanged = 1,
        OnWaypointStatusChanged = 2,
        OnNotificationRecieved = 3,
        OnTripFinished = 4,
        OnTripStatusChanged = 5
    }
 
    
    public LocationHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task JoinGroup(string groupName)
    {
        if (_groupCounts.ContainsKey(groupName))
        {
            _groupCounts[groupName]++;
        }
        else
        {
            _groupCounts[groupName] = 1;
        }
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        if (_groupCounts.ContainsKey(groupName))
        {
            _groupCounts[groupName]--;
            if (_groupCounts[groupName] <= 0)
            {
                _groupCounts.Remove(groupName);
            }
        }
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    private async Task SendMessageToGroup(string groupName, object data)
    {
        try
        {
            if (_groupCounts.ContainsKey(groupName))
            {
                await Clients.Groups(groupName).SendCoreAsync("change", [data]);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }

    public async Task SendToAccount(Guid accountId, SocketMethods methodName, object data)
    {
        await SendMessageToGroup($"account@{accountId}", new
        {
            Method = methodName,
            Data = data ?? new {}
        });
    }

    public async Task SendToTrip(Guid tripId, SocketMethods methodName, object data)
    {
        await SendMessageToGroup($"trip@{tripId}", new
        {
            Method = methodName,
            Data = data 
        });
    }

    [SignalRMethod]
    public async Task UpdateLocation(UpdateUserLocationCommand request)
    {
        await _mediator.Send(request);
    }
}