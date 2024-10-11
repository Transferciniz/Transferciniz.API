using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Queries.AccountVehicleQueries;

public class GetAccountVehiclesQuery: IRequest<List<AccountVehicle>>
{
}

public class GetAccountVehiclesQueryHandler: IRequestHandler<GetAccountVehiclesQuery, List<AccountVehicle>>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public GetAccountVehiclesQueryHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<List<AccountVehicle>> Handle(GetAccountVehiclesQuery request, CancellationToken cancellationToken)
    {
        return await _context.AccountVehicles
            .Where(x => x.AccountId == _userSession.Id)
            .Include(x => x.Vehicle)
            .ThenInclude(x => x.VehicleBrand)

            .Include(x => x.Vehicle)
            .ThenInclude(x => x.VehicleModel)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}