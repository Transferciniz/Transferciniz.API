using MediatR;
using Transferciniz.API.Entities;
using Transferciniz.API.Helpers;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AuthCommands;

public class RegisterCompanyCommand: IRequest<RegisterCompanyCommandResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string TaxNumber { get; set; }
}

public class RegisterCompanyCommandResponse
{
    public string Token { get; set; }
}

public class RegisterCompanyCommandHandler: IRequestHandler<RegisterCompanyCommand, RegisterCompanyCommandResponse>
{
    private readonly IUserSession _userSession;
    private readonly TransportationContext _context;
    private readonly IConfiguration _configuration;

    public RegisterCompanyCommandHandler(IUserSession userSession, TransportationContext context, IConfiguration configuration)
    {
        _userSession = userSession;
        _context = context;
        _configuration = configuration;
    }

    public async Task<RegisterCompanyCommandResponse> Handle(RegisterCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies.AddAsync(new Company
        {
            Id = Guid.NewGuid(),
            CompanyType = _userSession.UserType == UserType.Customer ? CompanyType.Customer : CompanyType.Transporter,
            TaxNumber = request.TaxNumber,
            Email = request.Email,
            Password = request.Password.ToMD5(),
            Name = request.Name,
            Address = request.Address,
            CommissionRate = 10,
            TaxRate = 20,
        }, cancellationToken);

        var session = await _context.Sessions.AddAsync(new Session
        {
            Id = Guid.NewGuid(),
            LastActivity = DateTime.UtcNow,
            RelatedId = company.Entity.Id,
            SessionType = SessionType.Company
        }, cancellationToken);

        return new RegisterCompanyCommandResponse
        {
            Token = company.Entity.GenerateToken(_configuration, session.Entity.Id)
        };
    }
}