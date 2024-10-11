using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.TripCommands;

public class CreateTripCommand: IRequest<CreateTripCommandResponse>
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public List<Guid> VehicleIds { get; set; }
    public List<VehicleWaypointPair> VehicleRoutePlans { get; set; }
}

public class VehicleWaypointPair
{
    public Guid VehicleId { get; set; }
    public decimal TotalLenghOfRoad { get; set; }
    public List<VehicleWaypoint> Waypoints { get; set; }
}

public class VehicleWaypoint
{
    public double Latitude { get; set; }
    public string Name { get; set; }
    public double Longitude { get; set; }
    public int Ordering { get; set; }
    public List<WaypointUser> Users { get; set; }
}

public class WaypointUser
{
    public Guid? UserId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}



public class CreateTripCommandResponse
{
    public Guid Id { get; set; }
}

public class CreateTripCommandHandler : IRequestHandler<CreateTripCommand, CreateTripCommandResponse>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public CreateTripCommandHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<CreateTripCommandResponse> Handle(CreateTripCommand request, CancellationToken cancellationToken)
    {
        var tripHeader = await _context.TripHeaders.AddAsync(new TripHeader
        {
            Id = Guid.NewGuid(),
            Fee = 0,
            Name = request.Name,
            AccountId = _userSession.Id,
            StartDate = request.StartDate.ToUniversalTime(),
            Status = TripStatus.WaitingApprove,
            TotalCost = 0,
            TotalTripCost = 0,
            TotalExtraServiceCost = 0,
        }, cancellationToken);
        
        
        await _context.Transactions.AddAsync(new Transaction
        {
            Id = Guid.NewGuid(),
            ProviderTransactionId = "not-implemented",
            Amount = tripHeader.Entity.TotalCost,
            UserId = _userSession.Id,
            TripId = tripHeader.Entity.Id,
        }, cancellationToken);

        var vehicleEntities = await _context.Vehicles
            .Where(x => request.VehicleIds.Contains(x.Id))
            .ToListAsync(cancellationToken: cancellationToken);
        
        foreach (var vehicleEntity in vehicleEntities)
        {
            var plans = request.VehicleRoutePlans.Where(x => x.VehicleId == vehicleEntity.Id).ToList();
            var accountVehicles = await _context.AccountVehicles
                .Where(x => x.VehicleId == vehicleEntity.Id)
                .ToListAsync(cancellationToken: cancellationToken);
            foreach (var plan in plans)
            {
                var accountVehicle = accountVehicles.First();
                accountVehicles.RemoveAt(0);
                var tripEntity = await _context.Trips.AddAsync(new Trip
                {
                    Id = Guid.NewGuid(),
                    TotalCost = (plan.TotalLenghOfRoad / 1000) * vehicleEntity.BasePrice,
                    TotalTripCost = (plan.TotalLenghOfRoad / 1000) * vehicleEntity.BasePrice,
                    StartDate = request.StartDate,
                    TripHeaderId = tripHeader.Entity.Id,
                    Fee = 0,
                    DriverId = accountVehicle.AccountId,
                    AccountVehicleId = accountVehicle.Id,
                    TotalExtraServiceCost = 0,
                    Status = TripStatus.Approved,
                    TripExtraServices = new List<TripExtraService>(),
                    WayPoints = new List<WayPoint>()
                }, cancellationToken);
                foreach (var waypoint in plan.Waypoints)
                {
                    var waypointEntity = await _context.WayPoints.AddAsync(new WayPoint
                    {
                        Id = Guid.NewGuid(),
                        Name = waypoint.Name,
                        TripId = tripEntity.Entity.Id,
                        Longitude = waypoint.Longitude,
                        Latitude = waypoint.Latitude,
                        Ordering = waypoint.Ordering,
                        WayPointUsers = new List<WayPointUser>()
                    }, cancellationToken);
                    foreach (var waypointUser in waypoint.Users)
                    {
                        await _context.WayPointUsers.AddAsync(new WayPointUser
                        {
                            Id = Guid.NewGuid(),
                            AccountId = waypointUser.UserId,
                            Name = waypointUser.Name,
                            Surname = waypointUser.Surname,
                            WayPointId = waypointEntity.Entity.Id
                        }, cancellationToken);
                    }
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return new CreateTripCommandResponse
        {
            Id = tripHeader.Entity.Id
        };
    }
}
