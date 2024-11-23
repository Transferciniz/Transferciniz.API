using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AccountVehicleCommands;

public class UpdateAccountVehicleStatusOnlineCommand: IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class UpdateAccountVehicleStatusCommandHandler: IRequestHandler<UpdateAccountVehicleStatusOnlineCommand, Unit>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public UpdateAccountVehicleStatusCommandHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<Unit> Handle(UpdateAccountVehicleStatusOnlineCommand request, CancellationToken cancellationToken)
    {
        var accountVehicle = await _context.AccountVehicles.FirstAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        accountVehicle.Status = VehicleStatus.Online;
        accountVehicle.DriverId = _userSession.Id;
        _context.AccountVehicles.Update(accountVehicle);
        
        await _context.SaveChangesAsync(cancellationToken);
        return new Unit();
    }
}