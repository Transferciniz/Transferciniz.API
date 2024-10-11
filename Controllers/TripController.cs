using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transferciniz.API.Commands.TripCommands;
using Transferciniz.API.DTOs;
using Transferciniz.API.Entities;
using Transferciniz.API.Queries.TripQueries;
using Transferciniz.API.Queries.VehicleFiltersQueries;

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
    public async Task<CreateTripCommandResponse> CreateTrip(CreateTripCommand request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<List<TripHeader>> GetTripHeaders(GetIncomingTripsQuery request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<List<Trip>> GetTripDetails([FromQuery] GetTripDetailsQuery request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<GetVehicleFiltersQueryResponse> GetVehicleFilters([FromQuery]GetVehicleFiltersQuery request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<List<TripHeader>> GetMyTrips([FromQuery] GetMyTripsQuery request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<List<GetVehicleTripsQueryResponse>> GetVehicleTrips([FromQuery] GetVehicleTripsQuery request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<Unit> StartTrip(StartTripCommand request) => await _mediator.Send(request);



    [HttpGet]
    public async Task<List<TripHeaderDto>> GetTripHeadersForCompany([FromQuery] GetTripHeadersForCompanyQuery request) => await _mediator.Send(request);

    [HttpGet]
    public async Task<List<TripDto>> GetTripDetailsForCompany([FromQuery] GetTripDetailsForCompanyQuery request) => await _mediator.Send(request);

}