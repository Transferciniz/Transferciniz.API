using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Unit = MediatR.Unit;

namespace Transferciniz.API.Commands.UserLocationCommands;

public class UpdateUserLocationCommand : IRequest<Unit>
{
    public double XLine { get; set; }
    public double YLine { get; set; }
    public Guid SessionId { get; set; }
    public Guid UserId { get; set; }
    public Guid? VehicleId { get; set; }
}

public class UpdateUserLocationCommandHandler : IRequestHandler<UpdateUserLocationCommand, Unit>
{
    private readonly TransportationContext _context;

    public UpdateUserLocationCommandHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateUserLocationCommand request, CancellationToken cancellationToken)
    {
        /*var session =
            await _context.Sessions.FirstOrDefaultAsync(x => x.Id == request.SessionId,
                cancellationToken: cancellationToken);
        if (session is not null)
        {
            var point = new GeometryFactory().ToGeometry(new Envelope(new Coordinate(request.XLine, request.YLine)));
            var userLocation = await _context.Accounts.FirstAsync(x => x.Id == request.UserId,
                cancellationToken: cancellationToken);
            userLocation.Latitude = request.XLine;
            userLocation.Longitude = request.YLine;
            userLocation.LastLocationUpdateTime = DateTime.UtcNow;

            if (request.VehicleId is not null)
            {
                var vehicleLocation = await _context.AccountVehicles.FirstAsync(x => x.VehicleId == request.VehicleId,
                    cancellationToken: cancellationToken);

                var distanceDifference = vehicleLocation.Location.Distance(point);
                if (distanceDifference > 1000)
                {
                    Console.WriteLine("Yeni araç mesafesi 1000m'lik çaptan daha fazla");
                    //TODO: Bunu bir notification'a bağla.
                }

                vehicleLocation.Location = point;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }*/
        return Unit.Value;
    }
}