using MediatR;
using Transferciniz.API.Entities;
using Transferciniz.API.Helpers;

namespace Transferciniz.API.Commands.AuthCommands;

public class RegisterUserCommand: IRequest<RegisterUserCommandResponse>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserType UserType { get; set; }
}

public class RegisterUserCommandResponse
{
    public string Token { get; set; }
}

public class RegisterUserCommandHandler: IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>
{
    private readonly TransportationContext _context;
    private readonly IConfiguration _configuration;
    
    public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.AddAsync(new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password.ToMD5(),
            UserType = request.UserType
        }, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        var session = await _context.Sessions.AddAsync(new Session
        {
            Id = Guid.NewGuid(),
            LastActivity = DateTime.UtcNow,
            RelatedId = user.Entity.Id,
            SessionType = SessionType.User
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterUserCommandResponse
        {
            Token = user.Entity.GenerateToken(_configuration, session.Entity.Id)
        };
    }
}