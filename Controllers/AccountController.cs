using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transferciniz.API.Commands.AccountCommands;
using Transferciniz.API.Entities;
using Transferciniz.API.Queries.AccountQueries;
using Transferciniz.API.Queries.CompanyQueries;

namespace Transferciniz.API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
[Authorize]
public class AccountController: ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<UploadProfilePictureCommandResponse> ChangeProfilePicture(UploadProfilePictureCommand request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<Unit> UpdateLocation(UpdateLocationCommand request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<List<MapTrackingResponse>> VehiclesForMap([FromQuery]GetAccountVehiclesForMapQuery request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<GetProfileQueryResponse> GetProfile(GetProfileQuery request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<List<ProfileLocationQueryResponse>> SearchProfileLocation([FromQuery]ProfileLocationQuery request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<AccountVehicle> AddVehicle(AddVehicleCommand request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<List<AccountNotification>> GetNotifications([FromQuery] GetNotificationsQuery request) => await _mediator.Send(request);
}