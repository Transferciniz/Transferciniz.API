using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transferciniz.API.Commands.VehicleModelCommands;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class VehicleModelController: ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleModelController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<VehicleModel> Create(CreateVehicleModelCommand request) => await _mediator.Send(request);
}