using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transferciniz.API.Commands.VehicleBrandCommands;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class VehicleBrandController: ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleBrandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<VehicleBrand> Create(CreateVehicleBrandCommand request) => await _mediator.Send(request);
}