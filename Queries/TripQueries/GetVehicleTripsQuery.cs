using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.DTOs;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Queries.TripQueries;

public class GetVehicleTripsQuery: IRequest<List<GetVehicleTripsQueryResponse>>
{
    public Guid AccountVehicleId { get; set; }
}

public class GetVehicleTripsQueryResponse
{
    public string Name { get; set; }
    public TripDto Trip { get; set; }
}

public class GetVehicleTripsQueryHandler: IRequestHandler<GetVehicleTripsQuery, List<GetVehicleTripsQueryResponse>>
{
    private readonly TransportationContext _context;

    public GetVehicleTripsQueryHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<List<GetVehicleTripsQueryResponse>> Handle(GetVehicleTripsQuery request, CancellationToken cancellationToken)
    {
        var statusQuery = new List<TripStatus>([TripStatus.Approved, TripStatus.Live]);
        var trips =  await _context.Trips.Where(x => x.AccountVehicleId == request.AccountVehicleId && statusQuery.Contains(x.Status) )
            .Include(x => x.AccountVehicle)
            .ThenInclude(x => x.Vehicle)
            .ThenInclude(x => x.VehicleBrand)
            
            .Include(x => x.AccountVehicle)
            .ThenInclude(x => x.Vehicle)
            .ThenInclude(x => x.VehicleModel)
            
            .Include(x => x.WayPoints)
            .ThenInclude(x => x.WayPointUsers)
            .OrderBy(x => x.StartDate)
            .ToListAsync(cancellationToken: cancellationToken);

        var tripHeaders = await _context.TripHeaders
            .Where(x => trips.Select(y => y.TripHeaderId).Distinct().Contains(x.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        return trips.Select(x => new GetVehicleTripsQueryResponse
        {
            Trip = x.ToDto(),
            Name = tripHeaders.First(y => y.Id == x.TripHeaderId).Name
        }).ToList();
        
    }
}