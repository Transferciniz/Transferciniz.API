using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Commands.AccountNotificationCommands;
using Transferciniz.API.Entities;
using Transferciniz.API.Hubs;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.TripCommands;

public class FinishTripCommand: IRequest<Unit>
{
    public Guid TripId { get; set; }
}

public class FinishTripCommandHandler: IRequestHandler<FinishTripCommand, Unit>
{
    private readonly TransportationContext _context;
    private readonly IMediator _mediator;
    private readonly IUserSession _userSession;
    private readonly LocationHub _locationHub;

    public FinishTripCommandHandler(TransportationContext context, IMediator mediator, IUserSession userSession, LocationHub locationHub)
    {
        _context = context;
        _mediator = mediator;
        _userSession = userSession;
        _locationHub = locationHub;
    }

    public async Task<Unit> Handle(FinishTripCommand request, CancellationToken cancellationToken)
    {
        var trip = await _context.Trips
            .Include(trip => trip.AccountVehicle)
            .Include(x => x.WayPoints)
            .FirstAsync(x => x.Id == request.TripId, cancellationToken: cancellationToken);
        trip.Status = TripStatus.Finished;
        foreach (var tripWayPoint in trip.WayPoints)
        {
            tripWayPoint.Status = WaypointStatus.Finished;
            _context.WayPoints.Update(tripWayPoint);
        }
        _context.Trips.Update(trip);
        await _context.SaveChangesAsync(cancellationToken);

        var companyAccountId = await _context.Accounts.Select(x => x.Id).FirstAsync(x => x == trip.AccountVehicle.AccountId, cancellationToken: cancellationToken);
        await _mediator.Send(new AddAccountNotificationCommand()
        {
            AccountId = companyAccountId,
            Message = $"Sürücünüz {_userSession.Name} {_userSession.Surname}, {trip.AccountVehicle.Plate} plakalı araçla bir transferi tamamlamıştır."
        }, cancellationToken);

        if (await _context.Trips.AnyAsync(x => x.TripHeaderId == trip.TripHeaderId && x.Status != TripStatus.Finished, cancellationToken: cancellationToken))
        {
            return Unit.Value;
        }

        var tripHeader = await _context.TripHeaders.FirstAsync(x => x.Id == trip.TripHeaderId, cancellationToken: cancellationToken);
        tripHeader.Status = TripStatus.Finished;
        _context.TripHeaders.Update(tripHeader);
        await _context.SaveChangesAsync(cancellationToken);

        await _locationHub.SendMessageToGroup($"vehicle@{trip.AccountVehicleId}", "onTripFinished", new { });
  
        return Unit.Value;

    }
}