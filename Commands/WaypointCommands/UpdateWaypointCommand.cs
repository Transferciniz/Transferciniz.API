using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.DTOs;
using Transferciniz.API.Entities;
using Transferciniz.API.Hubs;

namespace Transferciniz.API.Commands.WaypointCommands;

public class UpdateWaypointCommand: IRequest<Unit>
{
    public WayPointDto Waypoint { get; set; }
}

public class UpdateWaypointCommandHandler: IRequestHandler<UpdateWaypointCommand, Unit>
{
    private readonly TransportationContext _context;
    private readonly LocationHub _locationHub;

    public UpdateWaypointCommandHandler(TransportationContext context, LocationHub locationHub)
    {
        _context = context;
        _locationHub = locationHub;
    }

    public async Task<Unit> Handle(UpdateWaypointCommand request, CancellationToken cancellationToken)
    {
        var waypoint = await _context.WayPoints
            .Where(x => x.Id == request.Waypoint.Id)
            .Include(x => x.WayPointUsers)
            .FirstAsync(cancellationToken: cancellationToken);

        waypoint.IsCompleted = true;
        waypoint.Status = WaypointStatus.InProgress;
        _context.WayPoints.Update(waypoint);
        foreach (var user in request.Waypoint.Users)
        {
            var waypointUser = waypoint.WayPointUsers.First(x => x.Id == user.Id);
            waypointUser.IsCame = user.IsCame;
            _context.WayPointUsers.Update(waypointUser);
            if (user.AccountId is not null)
            {
                await _locationHub.SendToAccount((Guid)user.AccountId, LocationHub.SocketMethods.OnWaypointStatusChanged, new
                {
                    status = (int)WaypointStatus.InProgress
                });
            }
          
        }

        await _context.SaveChangesAsync(cancellationToken);
   
        return Unit.Value;
    }
}