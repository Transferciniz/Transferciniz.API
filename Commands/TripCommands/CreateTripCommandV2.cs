using Amazon.Runtime.Internal;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Commands.AccountNotificationCommands;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.TripCommands;

public class CreateTripCommandV2 : IRequest<Unit>
{
    public string Name { get; set; }
    public decimal Cost { get; set; }
    public List<DateTime> Dates { get; set; }
    public int Hour { get; set; }
    public int Minute { get; set; }
    public TimeType TimeType { get; set; }
    public List<CreateTripDto> Trips { get; set; }
}

public enum TimeType
{
    ArriveAtTime = 1,
    StartAtTime = 2
}

public class CreateTripDto
{
    public decimal Cost { get; set; }
    public Guid VehicleId { get; set; }
    public string Route { get; set; }
    public int Duration { get; set; }
    public TripDirection TripDirection { get; set; }
    public double StartLatitude { get; set; }
    public double StartLongitude { get; set; }
    public double EndLatitude { get; set; }
    public double EndLongitude { get; set; }
    public List<CreateWaypointDto> Waypoints { get; set; }
}

public class CreateWaypointDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Name { get; set; }
    public int Duration { get; set; }
    public List<CreateWaypointUserDto> Users { get; set; }
}

public class CreateWaypointUserDto
{
    public Guid? AccountId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}

public class CreateTripCommandV2Handler : IRequestHandler<CreateTripCommandV2, Unit>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;
    private readonly IMediator _mediator;

    public CreateTripCommandV2Handler(TransportationContext context, IUserSession userSession, IMediator mediator)
    {
        _context = context;
        _userSession = userSession;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(CreateTripCommandV2 request, CancellationToken cancellationToken)
    {
        foreach (var date in request.Dates)
        {
            var tripHeader = await _context.TripHeaders.AddAsync(new TripHeader
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                AccountId = _userSession.Id,
                Cost = request.Cost,
                Status = TripStatus.Approved
            }, cancellationToken);
            var vehicleIds = request.Trips.Select(x => x.VehicleId).Distinct().ToList();
            var accountVehicles = await _context.AccountVehicles
                .AsNoTracking()
                .Where(x => vehicleIds.Contains(x.VehicleId))
                .Select(x => new
                {
                    VehicleId = x.VehicleId,
                    Id = x.Id
                })
                .ToListAsync(cancellationToken: cancellationToken);
            foreach (var tripDto in request.Trips)
            {
                var startDate = DateTime.UtcNow;
                var endDate = DateTime.UtcNow;
                if (tripDto.TripDirection == TripDirection.From)
                {
                    startDate = request.TimeType == TimeType.ArriveAtTime
                        ? date
                            .ToUniversalTime()
                            .AddHours(request.Hour)
                            .AddMinutes(request.Minute)
                            .AddSeconds(tripDto.Duration * -1)
                        : date
                            .ToUniversalTime()
                            .AddHours(request.Hour)
                            .AddMinutes(request.Minute);
                    endDate = request.TimeType == TimeType.ArriveAtTime
                        ? date
                            .ToUniversalTime()
                            .AddHours(request.Hour)
                            .AddMinutes(request.Minute)
                        : date
                            .ToUniversalTime()
                            .AddHours(request.Hour)
                            .AddMinutes(request.Minute)
                            .AddSeconds(tripDto.Duration);
                }
                else
                {
                    startDate =  date
                            .ToUniversalTime()
                            .AddHours(request.Hour)
                            .AddMinutes(request.Minute);
                    endDate =  date
                            .ToUniversalTime()
                            .AddHours(request.Hour)
                            .AddMinutes(request.Minute)
                            .AddSeconds(tripDto.Duration);
                }
          
                var vehicle = accountVehicles.First(x => x.VehicleId == tripDto.VehicleId);
                accountVehicles.Remove(vehicle);
                var trip = await _context.Trips.AddAsync(new Trip
                {
                    Id = Guid.NewGuid(),
                    Cost = tripDto.Cost,
                    Status = TripStatus.Approved,
                    StartDate = startDate,
                    EndDate = endDate,
                    TripDirection = tripDto.TripDirection,
                    AccountVehicleId = vehicle.Id,
                    TripHeaderId = tripHeader.Entity.Id,
                    Route = tripDto.Route,
                    Bounds = new TripBound
                    {
                        Id = Guid.NewGuid(),
                        StartLatitude = tripDto.StartLatitude,
                        StartLongitude = tripDto.StartLongitude,
                        EndLatitude = tripDto.EndLatitude,
                        EndLongitude = tripDto.EndLongitude
                    }
                }, cancellationToken);
                foreach (var waypointDto in tripDto.Waypoints)
                {
                    var waypoint = await _context.WayPoints.AddAsync(new WayPoint
                    {
                        Id = Guid.NewGuid(),
                        Name = waypointDto.Name,
                        Status = WaypointStatus.Waiting,
                        IsCompleted = false,
                        TripId = trip.Entity.Id,
                        Longitude = waypointDto.Longitude,
                        Latitude = waypointDto.Latitude,
                        EstimatedTimeOfArrival = startDate.AddSeconds(waypointDto.Duration)
                    }, cancellationToken);
                    foreach (var userDto in waypointDto.Users)
                    {
                        await _context.WayPointUsers.AddAsync(new WayPointUser
                        {
                            Id = Guid.NewGuid(),
                            WayPointId = waypoint.Entity.Id,
                            Name = userDto.Name,
                            Surname = userDto.Surname,
                            AccountId = userDto.AccountId,
                            WillCome = true,
                            IsCame = false,
                        }, cancellationToken);
                    }
                }
            }
        }


        await _context.SaveChangesAsync(cancellationToken);
    
        return Unit.Value;
    }
}