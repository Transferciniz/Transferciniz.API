using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Commands.AccountNotificationCommands;
using Transferciniz.API.Entities;
using Transferciniz.API.Hubs;

namespace Transferciniz.API.Notifications;

public class UserLocationChangedNotification : INotification
{
    public Account Account { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class OnDriverLocationChanged : INotificationHandler<UserLocationChangedNotification>
{
    private readonly TransportationContext _context;
    private readonly LocationHub _locationHub;
    private readonly ILogger<OnDriverLocationChanged> _logger;
    private readonly IMediator _mediator;

    public OnDriverLocationChanged(TransportationContext context, LocationHub locationHub,
        ILogger<OnDriverLocationChanged> logger, IMediator mediator)
    {
        _context = context;
        _locationHub = locationHub;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(UserLocationChangedNotification notification, CancellationToken cancellationToken)
    {
        var waypointStatusList = new List<WaypointStatus>([
            WaypointStatus.OnRoad, WaypointStatus.Near500Mt, WaypointStatus.Near200Mt, WaypointStatus.Near1Km,
        ]);
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

                var trip = await _context.Trips
                    .Include(x => x.WayPoints)
                    .ThenInclude(x => x.WayPointUsers)
                    .Where(x => x.AccountVehicleId == accountVehicle.Id && x.Status == TripStatus.Live)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (trip is not null)
                {
                    await _locationHub.SendToTrip(trip.Id, LocationHub.SocketMethods.OnVehicleLocationChanged, new
                    {
                        notification.Latitude, notification.Longitude
                    });

                    var waypoints = trip.WayPoints
                        .Where(x => waypointStatusList.Contains(x.Status) && x.IsCompleted == false).ToList();

                    foreach (var waypoint in waypoints)
                    {
                        var currentWaypointStatus = waypoint.Status;
                        var newWaypointStatus = waypoint.Status;

                        var distance = CalculateDistanceAsMeter(waypoint, notification);
                        _logger.LogCritical($"Aracın {waypoint.Name} uzaklığı {distance} metredir");

                        if (distance <= 1000) newWaypointStatus = WaypointStatus.Near1Km;
                        if (distance <= 500) newWaypointStatus = WaypointStatus.Near500Mt;
                        if (distance <= 200) newWaypointStatus = WaypointStatus.Near200Mt;
                        if (distance <= 50) newWaypointStatus = WaypointStatus.OnWaypoint;

                        if (currentWaypointStatus != newWaypointStatus)
                        {
                            var notificationMessage = "";
                            switch (newWaypointStatus)
                            {
                                case WaypointStatus.Waiting:
                                    break;
                                case WaypointStatus.OnRoad:
                                    break;
                                case WaypointStatus.Near1Km:
                                    notificationMessage =
                                        "Aracınız size tahmini 1KM uzaklıktadır, hazır durumda beklemeniz için ilk çağrı.";
                                    break;
                                case WaypointStatus.Near500Mt:
                                    notificationMessage =
                                        "Aracınız mahallenize girmiştir, hazır durumda beklemeniz için ikinci çağrı.";
                                    break;
                                case WaypointStatus.Near200Mt:
                                    notificationMessage =
                                        "Aracınız durağınıza varmak üzeredir, hazır durumda beklemeniz için son çağrı.";
                                    break;
                                case WaypointStatus.OnWaypoint:
                                    notificationMessage = "Aracınız gelmiştir, lütfen aracınıza bininiz.";
                                    break;
                                case WaypointStatus.InProgress:
                                    break;
                                case WaypointStatus.Finished:
                                    break;
                                default:
                                    _logger.LogCritical(
                                        $"Yeni Durak Statüsü hiçbirine uymadı: {newWaypointStatus.ToString()}");
                                    break;
                            }

                            waypoint.Status = newWaypointStatus;
                            _context.WayPoints.Update(waypoint);
                            await _context.SaveChangesAsync(cancellationToken);
                            foreach (var account in waypoint.WayPointUsers.Where(x => x.AccountId.HasValue).ToList())
                            {
                                await _mediator.Send(new AddAccountNotificationCommand
                                {
                                    AccountId = (Guid)account.AccountId!,
                                    Message = notificationMessage
                                }, cancellationToken);
                                await _locationHub.SendToAccount((Guid)account.AccountId,
                                    LocationHub.SocketMethods.OnWaypointStatusChanged, new
                                    {
                                        status = (int)newWaypointStatus
                                    });
                            }
                        }
                    }
                }
            }
        }
    }

    private double CalculateDistanceAsMeter(WayPoint waypoint, UserLocationChangedNotification notification)
    {
        double R = 6371000; // Dünya'nın yarıçapı (metre)
        double dLat = (waypoint.Latitude - notification.Latitude) * Math.PI / 180;
        double dLon = (waypoint.Longitude - notification.Longitude) * Math.PI / 180;
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(notification.Latitude * Math.PI / 180) * Math.Cos(waypoint.Latitude * Math.PI / 180) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = R * c; // Mesafe metre olarak döner.
        return distance;
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
                    .SetProperty(account => account.Longitude, notification.Longitude),
                cancellationToken: cancellationToken);
        await _locationHub.SendToAccount(notification.Account.Id, LocationHub.SocketMethods.OnLocationChanged, new
        {
            notification.Latitude,
            notification.Longitude
        });

    }
}