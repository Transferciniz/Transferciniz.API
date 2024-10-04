using MediatR;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Commands.VehicleModelCommands;

public class CreateVehicleModelCommand: IRequest<VehicleModel>
{
    public int Capacity { get; set; }
    public int ExtraCapacity { get; set; }
    public string Name { get; set; }
    public Guid VehicleBrandId { get; set; }
}

public class CreateVehicleModelCommandHandler: IRequestHandler<CreateVehicleModelCommand, VehicleModel>
{
    private readonly TransportationContext _context;

    public CreateVehicleModelCommandHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<VehicleModel> Handle(CreateVehicleModelCommand request, CancellationToken cancellationToken)
    {
        var vehicleModel = await _context.VehicleModels.AddAsync(new VehicleModel
        {
            Id = Guid.NewGuid(),
            Capacity = request.Capacity,
            Name = request.Name,
            ExtraCapacity = request.ExtraCapacity,
            VehicleBrandId = request.VehicleBrandId
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return vehicleModel.Entity;
    }
}