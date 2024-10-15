using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.DTOs;
using Transferciniz.API.Services;

namespace Transferciniz.API.Queries.TripQueries;

public class GetTripHeadersForCustomerQuery: IRequest<List<TripHeaderDto>>
{
    
}

public class GetTripHeadersForCustomerQueryHandler: IRequestHandler<GetTripHeadersForCustomerQuery, List<TripHeaderDto>>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public GetTripHeadersForCustomerQueryHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<List<TripHeaderDto>> Handle(GetTripHeadersForCustomerQuery request, CancellationToken cancellationToken)
    {
        var waypointIds = await _context.WayPointUsers
            .Where(x => x.AccountId == _userSession.Id)
            .Select(x => x.WayPointId)
            .ToListAsync(cancellationToken: cancellationToken);

        var tripIds = (await _context.WayPoints
                .Where(x => waypointIds.Contains(x.Id))
                .Select(x => x.TripId)
                .ToListAsync(cancellationToken: cancellationToken))
            .Distinct()
            .ToList();

        var tripHeaderIds = (await _context.Trips
                .Where(x => tripIds.Contains(x.Id))
                .Select(x => x.TripHeaderId)
                .ToListAsync(cancellationToken: cancellationToken))
            .Distinct()
            .ToList();

        var tripHeaders = await _context.TripHeaders
            .Include(x => x.Trips)
            .Where(x => tripHeaderIds.Contains(x.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        return tripHeaders.Select(x => x.ToDto()).ToList();
    }
}