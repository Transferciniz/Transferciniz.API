using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Queries.AccountLocationQueries;

public class GetMyLocationsQuery: IRequest<List<AccountLocation>>
{
    
}

public class GetMyLocationsQueryHandler: IRequestHandler<GetMyLocationsQuery, List<AccountLocation>>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public GetMyLocationsQueryHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<List<AccountLocation>> Handle(GetMyLocationsQuery request, CancellationToken cancellationToken)
    {
        return await _context.AccountLocations.Where(x => x.AccountId == _userSession.Id).ToListAsync(cancellationToken: cancellationToken);
    }
}