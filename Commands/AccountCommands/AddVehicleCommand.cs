using MediatR;
using NetTopologySuite.Geometries;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AccountCommands;

public class AddVehicleCommand: IRequest<AccountVehicle>
{
    public Guid VehicleId { get; set; }
    public string Plate { get; set; }
}

public class AddVehicleCommandHandler: IRequestHandler<AddVehicleCommand, AccountVehicle>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public AddVehicleCommandHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<AccountVehicle> Handle(AddVehicleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AccountVehicles.AddAsync(new AccountVehicle
        {
            Id = Guid.NewGuid(),
            AccountId = _userSession.Id,
            VehicleId = request.VehicleId,
            Plate = request.Plate,
            Latitude = 0,
            Longitude = 0,
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Entity;
    }
}