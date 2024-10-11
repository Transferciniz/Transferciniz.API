using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Queries.AccountVehicleQueries;

public class GetAccountVehicleQuery: IRequest<AccountVehicle>
{
    public Guid Id { get; set; }
}

public class GetAccountVehicleQueryHandler : IRequestHandler<GetAccountVehicleQuery, AccountVehicle>
{
    private readonly TransportationContext _context;

    public GetAccountVehicleQueryHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<AccountVehicle> Handle(GetAccountVehicleQuery request, CancellationToken cancellationToken)
    {
        return await _context.AccountVehicles
            .Where(x => x.Id == request.Id)
            .Include(x => x.Vehicle)
            .ThenInclude(x => x.VehicleBrand)

            .Include(x => x.Vehicle)
            .ThenInclude(x => x.VehicleModel)
            .FirstAsync(cancellationToken: cancellationToken);
    }
}