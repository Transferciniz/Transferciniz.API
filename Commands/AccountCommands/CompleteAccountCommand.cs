using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Helpers;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AccountCommands;

public class CompleteAccountCommand: IRequest<CompleteAccountCommandResponse>
{
    
}

public class CompleteAccountCommandResponse
{
    public string Token { get; set; }
}

public class CompleteAccountCommandHandler : IRequestHandler<CompleteAccountCommand, CompleteAccountCommandResponse>
{
    private readonly IUserSession _session;
    private readonly TransportationContext _context;
    private readonly IConfiguration _configuration;

    public CompleteAccountCommandHandler(IUserSession session, TransportationContext context, IConfiguration configuration)
    {
        _session = session;
        _context = context;
        _configuration = configuration;
    }

    public async Task<CompleteAccountCommandResponse> Handle(CompleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.FirstAsync(x => x.Id == _session.Id, cancellationToken: cancellationToken);
        account.IsAccountCompleted = true;
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync(cancellationToken);

        var session = await _context.Sessions.FirstAsync(x => x.AccountId == account.Id, cancellationToken: cancellationToken);

        var token = account.GenerateToken(_configuration, session.Id);
        return new CompleteAccountCommandResponse
        {
            Token = token
        };
    }
}