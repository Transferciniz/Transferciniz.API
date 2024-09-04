using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Queries.CompanyQueries;

public class GetCompanyVehiclesQuery: IRequest<List<CompanyVehicle>>
{
    public Guid CompanyId { get; set; }
}

public class GetCompanyVehiclesQueryHandler: IRequestHandler<GetCompanyVehiclesQuery, List<CompanyVehicle>>
{
    private readonly IUserSession _userSession;
    private readonly TransportationContext _context;

    public GetCompanyVehiclesQueryHandler(IUserSession userSession, TransportationContext context)
    {
        _userSession = userSession;
        _context = context;
    }

    public async Task<List<CompanyVehicle>> Handle(GetCompanyVehiclesQuery request, CancellationToken cancellationToken)
    {
        return await _context.CompanyVehicles
            .Where(x => x.CompanyId == (Guid.Empty == request.CompanyId ? _userSession.CompanyId : request.CompanyId))
            .Include(x => x.Vehicle)
            .Include(x => x.VehicleExtraServices)
            .Include(x => x.VehicleSegmentFilters)
            .Include(x => x.VehicleTypeFilters)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}