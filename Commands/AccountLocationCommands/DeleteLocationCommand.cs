using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AccountLocationCommands;

public class DeleteLocationCommand: IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteLocationCommandHandler: IRequestHandler<DeleteLocationCommand, Unit>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public DeleteLocationCommandHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<Unit> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        if (await _context.AccountLocations.CountAsync(x => x.AccountId == _userSession.Id, cancellationToken: cancellationToken) == 1)
        {
            throw new Exception("En az bir adresinizin olması gerekmektedir, silmeden önce lütfen başka bir adres ekleyin.");
        }
        var location = await _context.AccountLocations.FirstOrDefaultAsync(x => x.Id == request.Id && x.AccountId == _userSession.Id, cancellationToken: cancellationToken);
        if (location is not null && location.IsDefault)
        {
            throw new Exception("Varsayılan adresinizi silemezsiniz, lütfen önce başka bir adresinizi varsayılan adres olarak ayarlayınız.");
        }

        if (location is null)
        {
            throw new Exception("Böyle bir adres bulunmamaktadır.");
        }

        _context.AccountLocations.Remove(location);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}