using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Transferciniz.API.Entities;
using Transferciniz.API.Queries.TripQueries;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.TripCommands;

public class CreateTripCommand: IRequest<CreateTripCommandResponse>
{
    public List<CombinationPricePair> SelectedVehicleCombinations { get; set; }

    public List<Waypoint> Waypoints { get; set; }
    public DateTime StartDate { get; set; }
    public string Name { get; set; }
    
}

public class Waypoint
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
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
        var startCoordinate = new Point(request.Waypoints[0].Latitude, request.Waypoints[0].Longitude);
        var tripHeader = await _context.TripHeaders.AddAsync(new TripHeader
        {
            Id = Guid.NewGuid(),
            Fee = 0,
            Name = request.Name,
            AccountId = _userSession.Id,
            StartDate = request.StartDate.ToUniversalTime(),
            Status = TripStatus.WaitingApprove,
            TotalCost = request.SelectedVehicleCombinations.Sum(x => x.TotalPrice),
            TotalTripCost = request.SelectedVehicleCombinations.Sum(x => x.TotalPrice),
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
        foreach (var combinationPricePair in request.SelectedVehicleCombinations)
        {
            foreach (var vehicleUsagePair in combinationPricePair.Vehicles)
            {
                var vehicles = await _context.AccountVehicles
                    .Where(x => x.VehicleId == vehicleUsagePair.Vehicle.Id)
                    .Select(x => new
                    {
                        x.Id,
                        Distance = startCoordinate.Distance(x.Location)
                    })
                    .OrderBy(x=> x.Distance)
                    .Skip(0)
                    .Take(vehicleUsagePair.Usage)
                    .ToListAsync(cancellationToken: cancellationToken);
                for (int i = 0; i < vehicleUsagePair.Usage; i++)
                {
                    var vehicle = vehicles[0];
                    vehicles.RemoveAt(0);
                    await _context.Trips.AddAsync(new Trip
                    {
                        Id = Guid.NewGuid(),
                        Fee = 0,
                        WayPoints = request.Waypoints.Select(x =>
                        {
                            var factory = new GeometryFactory();
                            var point =  factory.CreatePoint(new Coordinate(x.Latitude, x.Longitude));
                            return point as Geometry;
                        }).ToList(),
                        TotalCost = combinationPricePair.TotalPrice / vehicleUsagePair.Usage,
                        TotalTripCost = combinationPricePair.TotalPrice / vehicleUsagePair.Usage,
                        StartDate = request.StartDate.ToUniversalTime(),
                        TotalExtraServiceCost = combinationPricePair.TotalPrice / vehicleUsagePair.Usage,
                        TripHeaderId = tripHeader.Entity.Id,
                        CompanyVehicleId = vehicle.Id,
                    }, cancellationToken);
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
