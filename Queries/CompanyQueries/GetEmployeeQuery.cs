using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.DTOs;
using Transferciniz.API.Helpers;
using Transferciniz.API.Services;

namespace Transferciniz.API.Queries.CompanyQueries;

public class GetEmployeeQuery: IRequest<List<AccountDto>>
{
    
}

public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, List<AccountDto>>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _session;

    public GetEmployeeQueryHandler(TransportationContext context, IUserSession session)
    {
        _context = context;
        _session = session;
    }

    public async Task<List<AccountDto>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
    {
        return await _context.Accounts
            .Where(x => x.ParentAccountId == _session.Id)
            .Include(x => x.AccountLocations)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken: cancellationToken);
    }
}