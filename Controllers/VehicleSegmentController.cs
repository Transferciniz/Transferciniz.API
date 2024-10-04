using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transferciniz.API.Commands.VehicleSegmentCommands;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class VehicleSegmentController: ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleSegmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<VehicleSegment> Create(CreateVehicleSegmentCommand request) => await _mediator.Send(request);
}