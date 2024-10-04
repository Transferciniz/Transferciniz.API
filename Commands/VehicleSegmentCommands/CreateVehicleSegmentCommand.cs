using MediatR;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Commands.VehicleSegmentCommands;

public class CreateVehicleSegmentCommand: IRequest<VehicleSegment>
{
    public string Name { get; set; }
}

public class CreateVehicleSegmentCommandHandler: IRequestHandler<CreateVehicleSegmentCommand, VehicleSegment>
{
    private readonly TransportationContext _context;

    public CreateVehicleSegmentCommandHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<VehicleSegment> Handle(CreateVehicleSegmentCommand request, CancellationToken cancellationToken)
    {
        var vehicleSegment = await _context.VehicleSegments.AddAsync(new VehicleSegment
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return vehicleSegment.Entity;
    }
}