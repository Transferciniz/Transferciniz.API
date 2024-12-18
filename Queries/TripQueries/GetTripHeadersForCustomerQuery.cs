using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.DTOs;
using Transferciniz.API.Entities;
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
        var tripHeaders = await _context.TripHeaders
            .Where(th => th.Trips.Any(t =>
                t.WayPoints.Any(wp =>
                    wp.WayPointUsers.Any(wpu => wpu.AccountId == _userSession.Id))))
            .Include(x => x.Trips)
            .ThenInclude(t => t.AccountVehicle)
            .ThenInclude(av => av.Vehicle)
            .ThenInclude(v => v.VehicleModel)
            .Include(x => x.Trips)
            .ThenInclude(t => t.WayPoints)
            .ThenInclude(wp => wp.WayPointUsers)
            .ToListAsync(cancellationToken);

        var drivers = new List<Account>();
        drivers = tripHeaders.SelectMany(x => x.Trips).Where(x => x.DriverId.HasValue).Select(x => new Account
            {
                Id = x.DriverId ?? Guid.Empty
            })
            .Distinct().ToList();
        if (drivers.Count > 0)
        {
            drivers = await _context.Accounts.Where(x => drivers.Select(d => d.Id).Contains(x.Id)).ToListAsync(cancellationToken: cancellationToken);
        }

        return tripHeaders
            .Select(x => x.ToCustomerDto(_userSession.Id, drivers))
            .OrderBy(x => x.StartDate)
            .ToList();
        /*
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
            .ThenInclude(x => x.AccountVehicle)
            .ThenInclude(x => x.Vehicle)
            .ThenInclude(x => x.VehicleModel)
            .Where(x => tripHeaderIds.Contains(x.Id))
            
            .Include(x => x.Trips)
            .ThenInclude(x => x.WayPoints)
            .ThenInclude(x => x.WayPointUsers)
            
            .ToListAsync(cancellationToken: cancellationToken);

        return tripHeaders.Select(x => x.ToCustomerDto(_userSession.Id)).OrderBy(x => x.StartDate).ToList();
        */
    }
}