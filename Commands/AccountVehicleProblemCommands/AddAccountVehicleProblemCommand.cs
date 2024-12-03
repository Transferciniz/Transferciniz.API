using MediatR;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AccountVehicleProblemCommands;

public class AddAccountVehicleProblemCommand: IRequest<Unit>
{
    public Guid AccountVehicleId { get; set; }
    public string Message { get; set; }
}

public class AddAccountVehicleProblemCommandHanlder: IRequestHandler<AddAccountVehicleProblemCommand, Unit>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public AddAccountVehicleProblemCommandHanlder(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<Unit> Handle(AddAccountVehicleProblemCommand request, CancellationToken cancellationToken)
    {
        await _context.AccountVehicleProblems.AddAsync(new AccountVehicleProblem
        {
            Id = Guid.NewGuid(),
            AccountVehicleId = request.AccountVehicleId,
            Status = AccountVehicleProblemStatus.Pending,
            Message = request.Message,
            DriverId = _userSession.Id,
            CreatedAt = DateTime.UtcNow,
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}