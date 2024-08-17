using MediatR;
using Transferciniz.API.Entities;
using Transferciniz.API.Helpers;

namespace Transferciniz.API.Commands.UserCommands;

public class RegisterDriverCommand : IRequest<User>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsEnterprise { get; set; }
    public Guid? CompanyId { get; set; }
    public string CompanyName { get; set; }
    public string CompanyTaxNo { get; set; }
    public string CompanyAddress { get; set; }
}

public class RegisterDriverCommandHandler : IRequestHandler<RegisterDriverCommand, User>
{
    private readonly TransportationContext _context;

    public RegisterDriverCommandHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(RegisterDriverCommand request, CancellationToken cancellationToken)
    {
        var role = UserRole.Administrator;
        var companyId = Guid.Empty;
        if(request.CompanyId is not null)
        {
            role = UserRole.Employee;
        }
        
        if(request.CompanyName is not null)
        {
            var company = await _context.Companies.AddAsync(new Company
            {
                Id = Guid.NewGuid(),
                Name = request.CompanyName,
                TaxNumber = request.CompanyTaxNo,
                CompanyType = CompanyType.Transporter,
                Address = request.CompanyAddress
            }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            companyId = company.Entity.Id;
        }
        
        var user = await _context.Users.AddAsync(new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password.ToMD5(),
            Role = role,
            CompanyId = companyId,
            UserType = UserType.Driver
        }, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        var session = await _context.Sessions.AddAsync(new Session
        {
            Id = Guid.NewGuid(),
            LastActivity = DateTime.UtcNow,
            UserId = user.Entity.Id
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return user.Entity;
    }
}
