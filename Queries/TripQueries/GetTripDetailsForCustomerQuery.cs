using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.DTOs;
using Transferciniz.API.Services;

namespace Transferciniz.API.Queries.TripQueries;

public class GetTripDetailsForCustomerQuery: IRequest<List<TripDto>>
{
    public Guid TripHeaderId { get; set; }
}

public class GetTripDetailsForCustomerQueryHandler: IRequestHandler<GetTripDetailsForCustomerQuery, List<TripDto>>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _session;

    public GetTripDetailsForCustomerQueryHandler(TransportationContext context, IUserSession session)
    {
        _context = context;
        _session = session;
    }

    public async Task<List<TripDto>> Handle(GetTripDetailsForCustomerQuery request, CancellationToken cancellationToken)
    {
        var trips = await _context.Trips
            .Where(x => x.TripHeaderId == request.TripHeaderId)
            .Include(x => x.AccountVehicle)
            .ThenInclude(x => x.Vehicle)
            .ThenInclude(x => x.VehicleBrand)

            .Include(x => x.AccountVehicle)
            .ThenInclude(x => x.Vehicle)
            .ThenInclude(x => x.VehicleModel)

            .Include(x => x.WayPoints)
            .ThenInclude(x => x.WayPointUsers)
            .ThenInclude(x => x.Account)
            .ToListAsync(cancellationToken: cancellationToken);
        return trips.Select(x => x.ToDto()).ToList();
    }
}