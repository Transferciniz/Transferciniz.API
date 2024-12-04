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
            .ThenInclude(x => x.Account)
            .Select(x => new AccountVehicleProblem
            {
                Id = x.Id,
                AccountVehicleId = x.AccountVehicleId,
                AccountId = x.AccountId,
                Status = x.Status,
                AccountVehicleProblemHistories = x.AccountVehicleProblemHistories,
                Message = x.Message,
                CompletedAt = x.CompletedAt,
                CreatedAt = x.CreatedAt,
                Account = new Account
                {
                    Id = x.Account.Id,
                    Name = x.Account.Name,
                    Surname = x.Account.Surname,
                    ProfilePicture = x.Account.ProfilePicture,
                    AccountType = x.Account.AccountType
                }
            })
            .ToListAsync(cancellationToken: cancellationToken);
    }
}