using MediatR;
using Microsoft.EntityFrameworkCore;
using Skybrud.Essentials.Collections.Extensions;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Queries.TripQueries;

public class GetTripDetailsQuery: IRequest<List<Trip>>
{
    public Guid TripHeaderId { get; set; }
}

public class GetTripDetailsQueryHandler: IRequestHandler<GetTripDetailsQuery, List<Trip>>
{
    private readonly TransportationContext _context;

    public GetTripDetailsQueryHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<List<Trip>> Handle(GetTripDetailsQuery request, CancellationToken cancellationToken)
    {
       return await _context.Trips
            .Where(x => x.TripHeaderId == request.TripHeaderId)
            .Include(x => x.TripExtraServices)
            
            .Include(x => x.AccountVehicle)
            .ThenInclude(x => x.Vehicle)
            .ThenInclude(x => x.VehicleBrand)
            
            .Include(x => x.AccountVehicle)
            .ThenInclude(x => x.Vehicle)
            .ThenInclude(x => x.VehicleModel)
            
            .Include(x => x.WayPoints)
            .ThenInclude(x => x.WayPointUsers)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}