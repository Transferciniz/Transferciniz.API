using MediatR;
using NetTopologySuite.Geometries;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Commands.AccountCommands;

public class AddVehicleCommand: IRequest<AccountVehicle>
{
    public Guid AccountId { get; set; }
    public Guid VehicleId { get; set; }
    public string Plate { get; set; }
}

public class AddVehicleCommandHandler: IRequestHandler<AddVehicleCommand, AccountVehicle>
{
    private readonly TransportationContext _context;

    public AddVehicleCommandHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<AccountVehicle> Handle(AddVehicleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AccountVehicles.AddAsync(new AccountVehicle
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            VehicleId = request.VehicleId,
            Plate = request.Plate,
            Location = new Point(0,0)
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Entity;
    }
}