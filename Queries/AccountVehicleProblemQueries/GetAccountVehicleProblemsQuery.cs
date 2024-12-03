using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Queries.AccountVehicleProblemQueries;

public class GetAccountVehicleProblemsQuery: IRequest<List<AccountVehicleProblem>>
{
    public Guid AccountVehicleId { get; set; }
}

public class GetAccountVehicleProblemsQueryHandler: IRequestHandler<GetAccountVehicleProblemsQuery, List<AccountVehicleProblem>>
{
    private readonly TransportationContext _context;

    public GetAccountVehicleProblemsQueryHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<List<AccountVehicleProblem>> Handle(GetAccountVehicleProblemsQuery request, CancellationToken cancellationToken)
    {
        return await _context.AccountVehicleProblems
            .Where(x => x.AccountVehicleId == request.AccountVehicleId)
            .Include(x => x.Account)
            .Include(x => x.AccountVehicleProblemHistories)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}