using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Commands.TripCommands;

public class FinishTripCommand: IRequest<Unit>
{
    public Guid TripId { get; set; }
}

public class FinishTripCommandHandler: IRequestHandler<FinishTripCommand, Unit>
{
    private readonly TransportationContext _context;

    public FinishTripCommandHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(FinishTripCommand request, CancellationToken cancellationToken)
    {
        var trip = await _context.Trips.FirstAsync(x => x.Id == request.TripId, cancellationToken: cancellationToken);
        trip.Status = TripStatus.Finished;
        _context.Trips.Update(trip);
        await _context.SaveChangesAsync(cancellationToken);

        if (await _context.Trips.AnyAsync(x => x.TripHeaderId == trip.TripHeaderId && x.Status != TripStatus.Finished, cancellationToken: cancellationToken))
        {
            return Unit.Value;
        }

        var tripHeader = await _context.TripHeaders.FirstAsync(x => x.Id == trip.TripHeaderId, cancellationToken: cancellationToken);
        tripHeader.Status = TripStatus.Finished;
        _context.TripHeaders.Update(tripHeader);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}