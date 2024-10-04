using MediatR;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Commands.VehicleTypeCommands;

public class CreateVehicleTypeCommand: IRequest<VehicleType>
{
    public string Name { get; set; }
}

public class CreateVehicleTypeCommandHandler: IRequestHandler<CreateVehicleTypeCommand, VehicleType>
{
    private readonly TransportationContext _context;

    public CreateVehicleTypeCommandHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<VehicleType> Handle(CreateVehicleTypeCommand request, CancellationToken cancellationToken)
    {
        var vehicleType = await _context.VehicleTypes.AddAsync(new VehicleType
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return vehicleType.Entity;
    }
}