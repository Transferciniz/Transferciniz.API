using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Notifications;
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
    private readonly IMediator _mediator;

    public UpdateLocationCommandHandler(TransportationContext context, IUserSession session, IMediator mediator)
    {
        _context = context;
        _session = session;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == _session.Id, cancellationToken: cancellationToken);
        
        if (account is null) return Unit.Value;
        
        var notification = new UserLocationChangedNotification
        {
            Account = account,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };
        await _mediator.Publish(notification, cancellationToken);

        return Unit.Value;
    }
}