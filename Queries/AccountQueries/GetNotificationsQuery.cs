using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Queries.AccountQueries;

public class GetNotificationsQuery: IRequest<List<AccountNotification>>
{
    
}

public class GetNotificationsQueryHandler: IRequestHandler<GetNotificationsQuery, List<AccountNotification>>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public GetNotificationsQueryHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<List<AccountNotification>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        return await _context.AccountNotifications
            .Where(x => x.AccountId == _userSession.Id)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}