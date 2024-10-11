using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.DTOs;
using Transferciniz.API.Services;

namespace Transferciniz.API.Queries.TripQueries;

public class GetTripHeadersForCompanyQuery: IRequest<List<TripHeaderDto>>
{
    
}

public class GetTripHeadersForCompanyHandler: IRequestHandler<GetTripHeadersForCompanyQuery, List<TripHeaderDto>>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _session;

    public GetTripHeadersForCompanyHandler(TransportationContext context, IUserSession session)
    {
        _context = context;
        _session = session;
    }

    public async Task<List<TripHeaderDto>> Handle(GetTripHeadersForCompanyQuery request, CancellationToken cancellationToken)
    {
        var tripHeaderIds = (await _context.Trips
                .Include(x => x.AccountVehicle)
                .Where(x => x.AccountVehicle.AccountId == _session.Id)
                .Select(x => x.TripHeaderId)
                .ToListAsync(cancellationToken: cancellationToken))
            .Distinct()
            .ToList();

        var tripHeaders = await _context.TripHeaders
            .Where(x => tripHeaderIds.Contains(x.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        return tripHeaders.Select(x => x.ToDto()).ToList();
    }
}