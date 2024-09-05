using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transferciniz.API.Commands.AuthCommands;
using Transferciniz.API.Entities;
using Transferciniz.API.Queries.CompanyQueries;

namespace Transferciniz.API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
[Authorize]
public class CompanyController: ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<RegisterCompanyCommandResponse> Register(RegisterCompanyCommand request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<List<CompanyVehicle>> Vehicles(GetCompanyVehiclesQuery request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<List<MapTrackingResponse>> VehiclesForMap(GetCompanyVehiclesForMapQuery request) => await _mediator.Send(request);
}