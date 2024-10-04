using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transferciniz.API.Commands.VehicleTypeCommands;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class VehicleTypeController: ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<VehicleType> Create(CreateVehicleTypeCommand request) => await _mediator.Send(request);
}