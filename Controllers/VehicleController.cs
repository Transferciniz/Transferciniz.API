using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transferciniz.API.Commands.VehicleCommands;
using Transferciniz.API.Entities;
using Transferciniz.API.Queries.VehicleQueries;

namespace Transferciniz.API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class VehicleController: ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Vehicle> Create(CreateVehicleCommand request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<List<GetVehiclesQueryResponse>> GetVehicles([FromQuery] GetVehiclesQuery request) => await _mediator.Send(request);
}