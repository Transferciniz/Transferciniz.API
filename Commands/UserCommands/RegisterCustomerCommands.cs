using MediatR;
using Transferciniz.API.Entities;
using Transferciniz.API.Helpers;

namespace Transferciniz.API.Commands.UserCommands;

public class RegisterCustomerCommand: IRequest<User>
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

public class RegisterCustomerCommandsHandler : IRequestHandler<RegisterCustomerCommand, User>
{
    private readonly TransportationContext _context;

    public RegisterCustomerCommandsHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
    {
        var role = UserRole.Customer;
        var companyId = Guid.Empty;
        if(request.IsEnterprise && request.CompanyId is not null)
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
                CompanyType = CompanyType.Customer,
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
            UserType = UserType.Customer
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
