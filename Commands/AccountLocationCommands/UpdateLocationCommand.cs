using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AccountLocationCommands;

public class UpdateLocationCommand : AccountLocation, IRequest<Unit>
{
    
}

public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, Unit>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public UpdateLocationCommandHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<Unit> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        var location = await _context.AccountLocations.FirstAsync(x => x.Id == request.Id && x.AccountId == _userSession.Id, cancellationToken: cancellationToken);
        location.Address = request.Address;
        location.Name = request.Name;
        location.Latitude = request.Latitude;
        location.Longitude = request.Longitude;
        location.UpdatedAt = DateTime.UtcNow;

        _context.AccountLocations.Update(location);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
