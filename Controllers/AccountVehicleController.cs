using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transferciniz.API.Commands.AccountVehicleCommands;
using Transferciniz.API.Commands.AccountVehicleProblemCommands;
using Transferciniz.API.DTOs;
using Transferciniz.API.Entities;
using Transferciniz.API.Queries.AccountVehicleQueries;

namespace Transferciniz.API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
[Authorize(Policy = "SessionValidation")]
public class AccountVehicleController: ControllerBase
{
    private readonly IMediator _mediator;

    public AccountVehicleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<AccountVehicleDto> GetAccountVehicle([FromQuery] GetAccountVehicleQuery request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<Unit> OnlineVehicle(UpdateAccountVehicleStatusOnlineCommand request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<List<AccountVehicle>> GetMyVehicles([FromQuery]GetAccountVehiclesQuery request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<Unit> AddProblem(AddAccountVehicleProblemCommand request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<Unit> UpdateProblem(UpdateAccountVehicleProblemCommand request) => await _mediator.Send(request);
}