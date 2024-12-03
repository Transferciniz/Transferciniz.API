using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AccountVehicleProblemCommands;

public class UpdateAccountVehicleProblemCommand: IRequest<Unit>
{
    public Guid Id { get; set; }
    public AccountVehicleProblemStatus Status { get; set; }
}

public class UpdateAccountVehicleProblemCommandHandler : IRequestHandler<UpdateAccountVehicleProblemCommand, Unit>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public UpdateAccountVehicleProblemCommandHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<Unit> Handle(UpdateAccountVehicleProblemCommand request, CancellationToken cancellationToken)
    {
        var problem = await _context.AccountVehicleProblems.FirstAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        var fromStatus = problem.Status;

        problem.Status = request.Status;
        if (request.Status == AccountVehicleProblemStatus.Completed)
        {
            problem.CompletedAt = DateTime.UtcNow;
        }

        _context.AccountVehicleProblems.Update(problem);
        await _context.SaveChangesAsync(cancellationToken);

        await _context.AccountVehicleProblemHistories.AddAsync(new AccountVehicleProblemHistory
        {
            Id = Guid.NewGuid(),
            AccountId = _userSession.Id,
            CreatedAt = DateTime.UtcNow,
            FromStatus = fromStatus,
            ToStatus = request.Status,
            AccountVehicleProblemId = problem.Id
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}