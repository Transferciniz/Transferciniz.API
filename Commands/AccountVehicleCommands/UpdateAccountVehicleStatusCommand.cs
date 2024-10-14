using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Hubs;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AccountVehicleCommands;

public class UpdateAccountVehicleStatusCommand: IRequest<Unit>
{
    public Guid Id { get; set; }
    public VehicleStatus Status { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class UpdateAccountVehicleStatusCommandHandler: IRequestHandler<UpdateAccountVehicleStatusCommand, Unit>
{
    private readonly TransportationContext _context;
    private readonly LocationHub _locationHub;

    public UpdateAccountVehicleStatusCommandHandler(TransportationContext context, LocationHub locationHub)
    {
        _context = context;
        _locationHub = locationHub;
    }

    public async Task<Unit> Handle(UpdateAccountVehicleStatusCommand request, CancellationToken cancellationToken)
    {
        var accountVehicle = await _context.AccountVehicles
            .FirstAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        
        accountVehicle.Longitude = request.Longitude;
        accountVehicle.Latitude = request.Latitude;
        accountVehicle.Status = request.Status;

        _context.AccountVehicles.Update(accountVehicle);
        await _context.SaveChangesAsync(cancellationToken);

        var trip = await _context.Trips.FirstOrDefaultAsync(x => x.AccountVehicleId == accountVehicle.Id && x.Status == TripStatus.Live, cancellationToken: cancellationToken);
        if (trip is not null)
        {
            await _locationHub.SendMessageToGroup($"vehicle@{trip.Id}", "onVehicleLocationChange", new
            {
                request.Latitude, request.Longitude
            });

        }
        
        return new Unit();

    }
}