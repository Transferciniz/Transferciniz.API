using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Transferciniz.API.Commands.WaypointCommands;

public class UpdateUserWillComeCommand: IRequest<Unit>
{
    public Guid WaypointUserId { get; set; }
    public bool WillCome { get; set; }
}

public class UpdateUserWillComeCommandHandler: IRequestHandler<UpdateUserWillComeCommand, Unit>
{
    private readonly TransportationContext _context;

    public UpdateUserWillComeCommandHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateUserWillComeCommand request, CancellationToken cancellationToken)
    {
        await _context.WayPointUsers
            .Where(x => x.Id == request.WaypointUserId)
            .ExecuteUpdateAsync(x => x.SetProperty(
                wayPointUser => wayPointUser.WillCome, request.WillCome
            ), cancellationToken: cancellationToken);
        return Unit.Value;
    }
}