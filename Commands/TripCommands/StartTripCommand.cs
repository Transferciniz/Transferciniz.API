using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Commands.AccountNotificationCommands;
using Transferciniz.API.Entities;
using Transferciniz.API.Hubs;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.TripCommands;

public class StartTripCommand: IRequest<Unit>
{
    public Guid TripId { get; set; }
}

public class StartTripCommandHandler: IRequestHandler<StartTripCommand, Unit>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;
    private readonly IMediator _mediator;
    private readonly LocationHub _locationHub;

    public StartTripCommandHandler(TransportationContext context, IUserSession userSession, IMediator mediator, LocationHub locationHub)
    {
        _context = context;
        _userSession = userSession;
        _mediator = mediator;
        _locationHub = locationHub;
    }

    public async Task<Unit> Handle(StartTripCommand request, CancellationToken cancellationToken)
    {
        var trip = await _context.Trips
            .Include(x => x.AccountVehicle)
            .Include(x => x.WayPoints)
            .ThenInclude(x => x.WayPointUsers)
            .FirstAsync(x => x.Id == request.TripId, cancellationToken: cancellationToken);
        if (trip.Status == TripStatus.Live) return new Unit();
        trip.Status = TripStatus.Live;
        trip.DriverId = _userSession.Id;
        foreach (var tripWayPoint in trip.WayPoints)
        {
            tripWayPoint.Status = WaypointStatus.OnRoad;
        }
        _context.Trips.Update(trip);
    
        
        var tripHeader = await _context.TripHeaders.FirstAsync(x => x.Id == trip.TripHeaderId, cancellationToken: cancellationToken);
        tripHeader.Status = TripStatus.Live;
        _context.TripHeaders.Update(tripHeader);
        
        await _context.TripHistories.AddAsync(new TripHistory
        {
            Id = Guid.NewGuid(),
            TripId = trip.Id,
            DateTime = DateTime.UtcNow,
            Message = $"Transfer {_userSession.Name} {_userSession.Surname} tarafından başlatıldı."
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var accountIds = trip.WayPoints
            .SelectMany(x => x.WayPointUsers.Select(y => y.AccountId)).Where(x => x.HasValue)
            .ToList();
        foreach (var accountId in accountIds)
        {
            await _mediator.Send(new AddAccountNotificationCommand()
            {
                AccountId = (Guid)accountId,
                Message = $"Aracınız, {_userSession.Name} {_userSession.Surname} şoförlüğünde {trip.AccountVehicle.Plate} plakalı araçla yola çıkmıştır."
            }, cancellationToken);

            await _locationHub.SendToAccount((Guid)accountId, LocationHub.SocketMethods.OnTripStatusChanged, new { });
        }

        var companyAccountId = await _context.Accounts.Select(x => x.Id).FirstAsync(x => x == trip.AccountVehicle.AccountId, cancellationToken: cancellationToken);
        await _mediator.Send(new AddAccountNotificationCommand()
        {
            AccountId = companyAccountId,
            Message = $"Sürücünüz {_userSession.Name} {_userSession.Surname}, {trip.AccountVehicle.Plate} plakalı araçla transfer başlatmıştır."
        }, cancellationToken);
        return new Unit();
    }
}