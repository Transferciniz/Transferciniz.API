using MediatR;
using Transferciniz.API.Entities;
using Transferciniz.API.Helpers;

namespace Transferciniz.API.Commands.AuthCommands;

public class RegisterCommand: IRequest<RegisterCommandResponse>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public AccountType AccountType { get; set; }
    public string TaxNumber { get; set; }
    public string InvoiceAddress { get; set; }
    public string? ParentAccountId { get; set; }
}

public class RegisterCommandResponse
{
    public string Token { get; set; }
}

public class RegisterUserCommandHandler: IRequestHandler<RegisterCommand, RegisterCommandResponse>
{
    private readonly TransportationContext _context;
    private readonly IConfiguration _configuration;

    public RegisterUserCommandHandler(TransportationContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<RegisterCommandResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Accounts.AddAsync(new Account
        {
            Id = Guid.NewGuid(),
            ParentAccountId = request.ParentAccountId != "" ? Guid.Parse(request.ParentAccountId) : Guid.Empty,
            Name = request.Name,
            Surname = request.Surname,
            Username = request.Username,
            Email = request.Email,
            Password = request.Password.ToMD5(),
            AccountType = request.AccountType,
            CommissionRate = 10,
            TaxRate = 20,
            ProfilePicture = "",
            TaxNumber = request.TaxNumber,
            IsAccountCompleted = false,
            InvoiceAddress = request.InvoiceAddress,
        }, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        var session = await _context.Sessions.AddAsync(new Session
        {
            Id = Guid.NewGuid(),
            LastActivity = DateTime.UtcNow,
            AccountId = user.Entity.Id,
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterCommandResponse
        {
            Token = user.Entity.GenerateToken(_configuration, session.Entity.Id)
        };
    }
}