using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Queries.TripQueries;

public class GetIncomingTripsQuery: IRequest<List<TripHeader>>
{
    
}

public class GetIncomingTripsQueryHandler: IRequestHandler<GetIncomingTripsQuery, List<TripHeader>>
{
    private readonly IUserSession _userSession;
    private readonly TransportationContext _context;

    public GetIncomingTripsQueryHandler(IUserSession userSession, TransportationContext context)
    {
        _userSession = userSession;
        _context = context;
    }

    public async Task<List<TripHeader>> Handle(GetIncomingTripsQuery request, CancellationToken cancellationToken)
    {
        return await _context.TripHeaders
            .Where(x => x.StartDate > DateTime.UtcNow && x.StartDate < DateTime.UtcNow.AddDays(7))
            .Include(x => x.Trips)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}