using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AccountVehicleCommands;

public class ClearAccountVehicleDriverCommand: IRequest<Unit>
{
    
}

public class ClearAccountVehicleDriverCommandHandler: IRequestHandler<ClearAccountVehicleDriverCommand, Unit>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public ClearAccountVehicleDriverCommandHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<Unit> Handle(ClearAccountVehicleDriverCommand request, CancellationToken cancellationToken)
    {
        var accountVehicles = await _context.AccountVehicles.Where(x => x.DriverId == _userSession.Id)
            .ToListAsync(cancellationToken: cancellationToken);
        foreach (var accountVehicle in accountVehicles)
        {
            accountVehicle.DriverId = null;
            accountVehicle.Status = VehicleStatus.Offline;
        }
        _context.UpdateRange(accountVehicles);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}