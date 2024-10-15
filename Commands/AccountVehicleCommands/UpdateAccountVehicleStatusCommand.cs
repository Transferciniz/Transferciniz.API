using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Hubs;

namespace Transferciniz.API.Commands.AccountVehicleCommands;

public class UpdateAccountVehicleStatusOnlineCommand: IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class UpdateAccountVehicleStatusCommandHandler: IRequestHandler<UpdateAccountVehicleStatusOnlineCommand, Unit>
{
    private readonly TransportationContext _context;
    private readonly LocationHub _locationHub;

    public UpdateAccountVehicleStatusCommandHandler(TransportationContext context, LocationHub locationHub)
    {
        _context = context;
        _locationHub = locationHub;
    }

    public async Task<Unit> Handle(UpdateAccountVehicleStatusOnlineCommand request, CancellationToken cancellationToken)
    {
        var accountVehicle = await _context.AccountVehicles.FirstAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        accountVehicle.Status = VehicleStatus.Online;
        _context.AccountVehicles.Update(accountVehicle);
        
        await _context.SaveChangesAsync(cancellationToken);
        return new Unit();
    }
}