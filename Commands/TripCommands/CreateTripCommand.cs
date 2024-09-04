using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Transferciniz.API.Entities;
using Transferciniz.API.Queries.TripQueries;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.TripCommands;

public class CreateTripCommand: IRequest<TripHeader>
{
    public List<CombinationPricePair> Vehicles { get; set; }
    public Geometry StartCoordinate { get; set; }

    public string RouteJson { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
}

public class CreateTripCommandHandler : IRequestHandler<CreateTripCommand, TripHeader>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public CreateTripCommandHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<TripHeader> Handle(CreateTripCommand request, CancellationToken cancellationToken)
    {

        var tripHeader = await _context.TripHeaders.AddAsync(new TripHeader
        {
            Id = Guid.NewGuid(),
            Fee = 0,
            TotalCost = request.Vehicles.Sum(x => x.TotalPrice),
            TotalTripCost = request.Vehicles.Sum(x => x.TotalPrice),
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
        foreach (var combinationPricePair in request.Vehicles)
        {
            foreach (var vehicleUsagePair in combinationPricePair.Vehicles)
            {
                var vehicles = await _context.CompanyVehicles
                    .Where(x => x.VehicleId == vehicleUsagePair.Vehicle.Id)
                    .Select(x => new
                    {
                        x.Id,
                        Distance = request.StartCoordinate.Distance(x.Location)
                    })
                    .OrderBy(x=> x.Distance)
                    .Skip(0)
                    .Take(vehicleUsagePair.Usage)
                    .ToListAsync(cancellationToken: cancellationToken);
                for (int i = 1; i < vehicleUsagePair.Usage; i++)
                {
                    var vehicle = vehicles[0];
                    vehicles.RemoveAt(0);
                    await _context.Trips.AddAsync(new Trip
                    {
                        Id = Guid.NewGuid(),
                        Fee = 0,
                        TotalCost = combinationPricePair.TotalPrice / vehicleUsagePair.Usage,
                        RouteJson = request.RouteJson,
                        TotalTripCost = combinationPricePair.TotalPrice / vehicleUsagePair.Usage,
                        StartDate = request.StartDate,
                        EndDate = request.EndDate,
                        TotalExtraServiceCost = combinationPricePair.TotalPrice / vehicleUsagePair.Usage,
                        TripHeaderId = tripHeader.Entity.Id,
                        CompanyVehicleId = vehicle.Id,
                    }, cancellationToken);
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return tripHeader.Entity;

    }
}
