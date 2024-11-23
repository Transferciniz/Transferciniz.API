using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Hubs;

namespace Transferciniz.API.Notifications;

public class UserLocationChangedNotification: INotification
{
    public Account Account { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class OnDriverLocationChanged: INotificationHandler<UserLocationChangedNotification>
{
    private readonly TransportationContext _context;
    private readonly LocationHub _locationHub;

    public OnDriverLocationChanged(TransportationContext context, LocationHub locationHub)
    {
        _context = context;
        _locationHub = locationHub;
    }

    public async Task Handle(UserLocationChangedNotification notification, CancellationToken cancellationToken)
    {
        if (notification.Account.AccountType == AccountType.Driver)
        {
            var accountVehicle = await _context.AccountVehicles
                .Where(x => x.DriverId == notification.Account.Id)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (accountVehicle is not null)
            {
                accountVehicle.Latitude = notification.Latitude;
                accountVehicle.Longitude = notification.Longitude;
                _context.AccountVehicles.Update(accountVehicle);
                await _context.SaveChangesAsync(cancellationToken);
                

                await _locationHub.SendMessageToGroup($"vehicle@{accountVehicle.Id}", "onVehicleLocationChanged", new
                {
                    notification.Latitude, notification.Longitude
                });
            }
        }
    }
}

public class OnUserLocationChanged : INotificationHandler<UserLocationChangedNotification>
{
    private readonly TransportationContext _context;
    private readonly LocationHub _locationHub;

    public OnUserLocationChanged(TransportationContext context, LocationHub locationHub)
    {
        _context = context;
        _locationHub = locationHub;
    }

    public async Task Handle(UserLocationChangedNotification notification, CancellationToken cancellationToken)
    {
        await _context.Accounts
            .Where(x => x.Id == notification.Account.Id)
            .ExecuteUpdateAsync(x => x
                .SetProperty(account => account.Latitude, notification.Latitude)
                .SetProperty(account => account.Longitude, notification.Longitude), cancellationToken: cancellationToken);
        await _locationHub.SendMessageToGroup($"account@{notification.Account.Id}", "onAccountLocationChanged", new
        {
            notification.Latitude,
            notification.Longitude
        });
    }
}