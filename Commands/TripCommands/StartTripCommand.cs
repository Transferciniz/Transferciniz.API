using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Commands.TripCommands;

public class StartTripCommand: IRequest<Unit>
{
    public Guid TripId { get; set; }
}

public class StartTripCommandHandler: IRequestHandler<StartTripCommand, Unit>
{
    private readonly TransportationContext _context;

    public StartTripCommandHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(StartTripCommand request, CancellationToken cancellationToken)
    {
        var trip = await _context.Trips.FirstAsync(x => x.Id == request.TripId, cancellationToken: cancellationToken);
        if (trip.Status == TripStatus.Live) return new Unit();
        trip.Status = TripStatus.Live;
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