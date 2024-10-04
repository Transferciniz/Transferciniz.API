using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Helpers;

namespace Transferciniz.API.Commands.AuthCommands;

public class LoginCommand : IRequest<LoginCommandResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginCommandResponse
{
    public string Token { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResponse>
{
    private readonly TransportationContext _context;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(TransportationContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(
            x =>
                x.Password == request.Password.ToMD5() &&
                x.Email == request.Email,
            cancellationToken: cancellationToken);
        if (account is null)
        {
            throw new Exception("Mail yada parolanız hatalı");
        }

        var session = await _context.Sessions.FirstOrDefaultAsync(x => x.AccountId == account.Id,
            cancellationToken: cancellationToken);
        if (session is not null)
        {
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var newSession = await _context.Sessions.AddAsync(new Session
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            LastActivity = DateTime.UtcNow
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return new LoginCommandResponse
        {
            Token = account.GenerateToken(_configuration, newSession.Entity.Id)
        };
    }
}