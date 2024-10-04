using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Queries.TripQueries;

public class GetMyTripsQuery: IRequest<List<TripHeader>>
{
    
}

public class GetMyTripsQueryHandler : IRequestHandler<GetMyTripsQuery, List<TripHeader>>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _session;

    public GetMyTripsQueryHandler(TransportationContext context, IUserSession session)
    {
        _context = context;
        _session = session;
    }

    public async Task<List<TripHeader>> Handle(GetMyTripsQuery request, CancellationToken cancellationToken)
    {
        return await _context.TripHeaders.Where(x => x.AccountId == _session.Id)
            .OrderBy(x => x.StartDate)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}