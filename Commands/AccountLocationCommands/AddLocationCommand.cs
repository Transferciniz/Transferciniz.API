using MediatR;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AccountLocationCommands;

public class AddLocationCommand: IRequest<Unit>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsDefault { get; set; }
}

public class AddLocationCommandHandler: IRequestHandler<AddLocationCommand, Unit>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public AddLocationCommandHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<Unit> Handle(AddLocationCommand request, CancellationToken cancellationToken)
    {
        await _context.AccountLocations.AddAsync(new AccountLocation
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Address = request.Address,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            AccountId = _userSession.Id,
            IsDefault = request.IsDefault,
            UpdatedAt = DateTime.UtcNow
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}