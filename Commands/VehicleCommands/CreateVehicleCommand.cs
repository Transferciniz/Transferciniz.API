using MediatR;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Commands.VehicleCommands;

public class CreateVehicleCommand: IRequest<Vehicle>
{
    public decimal BasePrice { get; set; }
    public Guid BrandId { get; set; }
    public Guid ModelId { get; set; }
}

public class CreateVehicleCommandHandler: IRequestHandler<CreateVehicleCommand, Vehicle>
{
    private readonly TransportationContext _context;

    public CreateVehicleCommandHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<Vehicle> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Vehicles.AddAsync(new Vehicle
        {
            Id = Guid.NewGuid(),
            BasePrice = request.BasePrice,
            VehicleBrandId = request.BrandId,
            VehicleModelId = request.ModelId
        });
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Entity;
    }
}