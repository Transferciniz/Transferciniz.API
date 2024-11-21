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

    public async Task SendMessageToGroup(string groupName, string methodName, object data)
    {
        try
        {
            if (_groupCounts.ContainsKey(groupName))
            {
                await Clients.Groups(groupName).SendCoreAsync(methodName, [data]);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }

    [SignalRMethod]
    public async Task UpdateLocation(UpdateUserLocationCommand request)
    {
        await _mediator.Send(request);
    }
}