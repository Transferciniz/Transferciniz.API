using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Hubs;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AccountCommands;

public class UpdateLocationCommand: IRequest<Unit>
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}

public class UpdateLocationCommandHandler: IRequestHandler<UpdateLocationCommand, Unit>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _session;
    private readonly LocationHub _locationHub;

    public UpdateLocationCommandHandler(TransportationContext context, IUserSession session, LocationHub locationHub)
    {
        _context = context;
        _session = session;
        _locationHub = locationHub;
    }

    public async Task<Unit> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == _session.Id, cancellationToken: cancellationToken);
        if(account is not null)
        {
            account.Latitude = request.Latitude;
            account.Longitude = request.Longitude;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync(cancellationToken);
            await _locationHub.SendMessageToGroup($"{_session.Id}", "onUserLocationChange", new
            {
                request.Latitude,
                request.Longitude
            });
            if (account.AccountType == AccountType.Driver)
            {
                var trip = await _context.Trips.FirstOrDefaultAsync(x => x.DriverId == _session.Id && x.Status == TripStatus.Live, cancellationToken: cancellationToken);
                if(trip is not null)
                {
                    await _locationHub.SendMessageToGroup($"vehicle@{trip.Id}", "onVehicleLocationChange", new
                    {
                        request.Latitude,
                        request.Longitude
                    });
                }
            }
        }

        return Unit.Value;
    }
}