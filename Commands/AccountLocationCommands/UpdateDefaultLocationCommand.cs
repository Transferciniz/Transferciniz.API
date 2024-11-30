using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AccountLocationCommands;

public class UpdateDefaultLocationCommand: IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class UpdateDefaultLocationCommandHandler : IRequestHandler<UpdateDefaultLocationCommand, Unit>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public UpdateDefaultLocationCommandHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<Unit> Handle(UpdateDefaultLocationCommand request, CancellationToken cancellationToken)
    {
        await _context.AccountLocations
            .Where(x => x.AccountId == _userSession.Id)
            .ExecuteUpdateAsync(x => x
                .SetProperty(accountLocation => accountLocation.IsDefault, false), cancellationToken: cancellationToken);

        await _context.AccountLocations
            .Where(x => x.Id == request.Id)
            .ExecuteUpdateAsync(x => x
                .SetProperty(accountLocation => accountLocation.IsDefault, true), cancellationToken: cancellationToken);
        
        return Unit.Value;
    }
}
