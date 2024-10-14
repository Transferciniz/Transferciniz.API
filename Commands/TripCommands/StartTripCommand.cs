using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
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

    public StartTripCommandHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<Unit> Handle(StartTripCommand request, CancellationToken cancellationToken)
    {
        var trip = await _context.Trips.FirstAsync(x => x.Id == request.TripId, cancellationToken: cancellationToken);
        if (trip.Status == TripStatus.Live) return new Unit();
        trip.Status = TripStatus.Live;
        trip.DriverId = _userSession.Id;
        _context.Trips.Update(trip);
        await _context.TripHistories.AddAsync(new TripHistory
        {
            Id = Guid.NewGuid(),
            TripId = trip.Id,
            DateTime = DateTime.UtcNow,
            Message = "Transfer sürücü tarafından başlatıldı."
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return new Unit();
    }
}