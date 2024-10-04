using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Queries.CompanyQueries;

public class GetAccountVehiclesQuery: IRequest<List<AccountVehicle>>
{
    public Guid CompanyId { get; set; }
}

public class GetCompanyVehiclesQueryHandler: IRequestHandler<GetAccountVehiclesQuery, List<AccountVehicle>>
{
    private readonly IUserSession _userSession;
    private readonly TransportationContext _context;

    public GetCompanyVehiclesQueryHandler(IUserSession userSession, TransportationContext context)
    {
        _userSession = userSession;
        _context = context;
    }

    public async Task<List<AccountVehicle>> Handle(GetAccountVehiclesQuery request, CancellationToken cancellationToken)
    {
        return await _context.AccountVehicles
            .Where(x => x.AccountId == (Guid.Empty == request.CompanyId ? _userSession.Id : request.CompanyId))
            .Include(x => x.Vehicle)
            .Include(x => x.VehicleExtraServices)
            .Include(x => x.VehicleSegmentFilters)
            .Include(x => x.VehicleTypeFilters)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}