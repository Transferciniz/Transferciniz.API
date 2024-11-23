using MediatR;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.VehicleModelCommands;

public class CreateVehicleModelCommand: IRequest<VehicleModel>
{
    public int Capacity { get; set; }
    public int ExtraCapacity { get; set; }
    public string Name { get; set; }
    public Guid VehicleBrandId { get; set; }
    public IFormFile Photo { get; set; }
}

public class CreateVehicleModelCommandHandler: IRequestHandler<CreateVehicleModelCommand, VehicleModel>
{
    private readonly TransportationContext _context;
    private readonly IS3Service _s3;

    public CreateVehicleModelCommandHandler(TransportationContext context, IS3Service s3)
    {
        _context = context;
        _s3 = s3;
    }

    public async Task<VehicleModel> Handle(CreateVehicleModelCommand request, CancellationToken cancellationToken)
    {
        var photoUrl = await _s3.UploadFileToSpacesAsync(request.Photo);
        var vehicleModel = await _context.VehicleModels.AddAsync(new VehicleModel
        {
            Id = Guid.NewGuid(),
            Capacity = request.Capacity,
            Name = request.Name,
            ExtraCapacity = request.ExtraCapacity,
            VehicleBrandId = request.VehicleBrandId,
            Photo = photoUrl
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return vehicleModel.Entity;
    }
}