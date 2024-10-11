using MediatR;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using Transferciniz.API.Commands.UserLocationCommands;

namespace Transferciniz.API.Hubs;

[SignalRHub]
public class LocationHub: Hub
{
    private readonly IMediator _mediator;

    public LocationHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task JoinGroup(string groupName) => await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

    public async Task LeaveGroup(string groupName) => await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

    public async Task SendMessageToGroup(string groupName, string methodName, object data)
    {
        await Clients.Groups(groupName).SendCoreAsync(methodName, [data]);
    }

    [SignalRMethod]
    public async Task UpdateLocation(UpdateUserLocationCommand request)
    {
        await _mediator.Send(request);
    }
}