using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transferciniz.API.Commands.AccountCommands;
using Transferciniz.API.Commands.AccountLocationCommands;
using Transferciniz.API.DTOs;
using Transferciniz.API.Entities;
using Transferciniz.API.Queries.AccountLocationQueries;
using Transferciniz.API.Queries.AccountQueries;
using Transferciniz.API.Queries.CompanyQueries;
using UpdateLocationCommand = Transferciniz.API.Commands.AccountCommands.UpdateLocationCommand;

namespace Transferciniz.API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
[Authorize(Policy = "SessionValidation")]
public class AccountController: ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<List<MapTrackingResponse>> VehiclesForMap([FromQuery]GetAccountVehiclesForMapQuery request) => await _mediator.Send(request);
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<GetProfileQueryResponse> GetProfile([FromQuery]GetProfileQuery request) => await _mediator.Send(request);
    
    [HttpGet]
    public async Task<List<ProfileLocationQueryResponse>> SearchProfileLocation([FromQuery]ProfileLocationQuery request) => await _mediator.Send(request);
    
    [HttpGet]
    public async Task<List<AccountNotification>> GetNotifications([FromQuery] GetNotificationsQuery request) => await _mediator.Send(request);
    
    [HttpGet]
    public async Task<List<AccountLocation>> GetMyLocations([FromQuery] GetMyLocationsQuery request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<UploadProfilePictureCommandResponse> ChangeProfilePicture([FromForm]UploadProfilePictureCommand request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<Unit> UpdateLocation(UpdateLocationCommand request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<AccountVehicle> AddVehicle(AddVehicleCommand request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<Unit> AddLocation(AddLocationCommand request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<Unit> UpdateMyLocation(UpdateLocationCommand request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<Unit> DeleteMyLocation(DeleteLocationCommand request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<Unit> UpdateDefaultLocation(UpdateDefaultLocationCommand request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<CompleteAccountCommandResponse> CompleteAccount(CompleteAccountCommand request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<List<AccountDto>> GetEmployee([FromQuery]GetEmployeeQuery request) => await _mediator.Send(request);


}