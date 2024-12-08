using MediatR;
using Transferciniz.API.Entities;
using Transferciniz.API.Hubs;

namespace Transferciniz.API.Commands.AccountNotificationCommands;

public class AddAccountNotificationCommand: IRequest<Unit>
{
    public Guid AccountId { get; set; }
    public string Message { get; set; }
}

public class AddAccountNotificationCommandHandler: IRequestHandler<AddAccountNotificationCommand, Unit>
{
    private readonly TransportationContext _context;
    private readonly LocationHub _locationHub;

    public AddAccountNotificationCommandHandler(TransportationContext context, LocationHub locationHub)
    {
        _context = context;
        _locationHub = locationHub;
    }

    public async Task<Unit> Handle(AddAccountNotificationCommand request, CancellationToken cancellationToken)
    {
        await _context.AccountNotifications.AddAsync(new AccountNotification
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            CreatedAt = DateTime.UtcNow,
            IsViewed = false,
            Message = request.Message
        }, cancellationToken);

        await _locationHub.SendToAccount(request.AccountId, LocationHub.SocketMethods.OnNotificationRecieved,
            new { request.Message });

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}