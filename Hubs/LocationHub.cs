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

    [SignalRMethod]
    public async Task UpdateLocation(UpdateUserLocationCommand request)
    {
        await _mediator.Send(request);
    }
}