using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Helpers;

namespace Transferciniz.API.Commands.AuthCommands;

public class LoginCommand : IRequest<LoginCommandResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public SessionType SessionType { get; set; }
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
        if (request.SessionType == SessionType.User)
        {
            var user = await _context.Users.FirstOrDefaultAsync(
                x =>
                    x.Password == request.Password.ToMD5() &&
                    x.Email == request.Email,
                cancellationToken: cancellationToken);
            if (user is null)
            {
                throw new Exception("Mail yada parolanız hatalı");
            }

            var session = await _context.Sessions.FirstOrDefaultAsync(x => x.RelatedId == user.Id,
                cancellationToken: cancellationToken);
            if (session is not null)
            {
                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var newSession = await _context.Sessions.AddAsync(new Session
            {
                Id = Guid.NewGuid(),
                RelatedId = user.Id,
                SessionType = SessionType.User,
                LastActivity = DateTime.UtcNow
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            return new LoginCommandResponse
            {
                Token = user.GenerateToken(_configuration, newSession.Entity.Id)
            };
        }

        var company = await _context.Companies.FirstOrDefaultAsync(x =>
                x.Email == request.Email &&
                x.Password == request.Password.ToMD5(), cancellationToken: cancellationToken);
        if(company is null)
        {
            throw new Exception("Mail yada parolanız hatalı");
        }

        var companySession = await _context.Sessions.FirstOrDefaultAsync(x =>
            x.RelatedId == company.Id, cancellationToken: cancellationToken);
        if(companySession is not null)
        {
            _context.Sessions.Remove(companySession);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var newCompanySession = await _context.Sessions.AddAsync(new Session
        {
            Id = Guid.NewGuid(),
            RelatedId = company.Id,
            LastActivity = DateTime.UtcNow,
            SessionType = SessionType.Company
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return new LoginCommandResponse
        {
            Token = company.GenerateToken(_configuration, newCompanySession.Entity.Id)
        };
    }
}