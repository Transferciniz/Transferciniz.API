using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Helpers;

namespace Transferciniz.API.Commands.UserCommands;

public class LoginCommand: IRequest<LoginCommandResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public UserType UserType { get; set; }
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
        var user = await _context.Users.FirstOrDefaultAsync(
            x => 
                x.UserType == request.UserType &&
                x.Password == request.Password.ToMD5() &&
                x.Email == request.Email,
            cancellationToken: cancellationToken);
        if(user is null)
        {
            throw new Exception("Mail yada parolanız hatalı");
        }

        var session = await _context.Sessions.FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken: cancellationToken);
        if (session is not null)
        {
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var newSession = await _context.Sessions.AddAsync(new Session
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            LastActivity = DateTime.UtcNow
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return new LoginCommandResponse
        {
            Token = user.GenerateToken(_configuration, newSession.Entity.Id)
        };
    }
}

