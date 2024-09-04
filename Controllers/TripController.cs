using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transferciniz.API.Commands.TripCommands;
using Transferciniz.API.Entities;
using Transferciniz.API.Queries.TripQueries;

namespace Transferciniz.API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
[Authorize]
public class TripController: ControllerBase
{
    private readonly IMediator _mediator;

    public TripController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<GetAvailableVehiclesQueryResponse> GetAvailableVehicles(GetAvailableVehiclesQuery request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<TripHeader> CreateTrip(CreateTripCommand request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<List<TripHeader>> GetIncomingTripHeaders(GetIncomingTripsQuery request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<List<Trip>> GetTrips([FromQuery] GetTripDetailsQuery request) => await _mediator.Send(request);

}