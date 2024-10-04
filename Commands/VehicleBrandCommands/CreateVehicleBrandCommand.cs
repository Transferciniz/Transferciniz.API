using MediatR;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Commands.VehicleBrandCommands;

public class CreateVehicleBrandCommand: IRequest<VehicleBrand>
{
    public string Name { get; set; }
}

public class CreateVehicleBrandCommandHandler: IRequestHandler<CreateVehicleBrandCommand, VehicleBrand>
{
    private readonly TransportationContext _context;

    public CreateVehicleBrandCommandHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<VehicleBrand> Handle(CreateVehicleBrandCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _context.VehicleBrands.AddAsync(new VehicleBrand
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return vehicle.Entity;
    }
}