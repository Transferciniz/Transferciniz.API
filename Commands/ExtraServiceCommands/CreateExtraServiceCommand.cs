using MediatR;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Commands.ExtraServiceCommands;

public class CreateExtraServiceCommand: IRequest<ExtraService>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class CreateExtraServiceCommandHandler : IRequestHandler<CreateExtraServiceCommand, ExtraService>
{
    private readonly TransportationContext _context;

    public CreateExtraServiceCommandHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<ExtraService> Handle(CreateExtraServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ExtraServices.AddAsync(new ExtraService
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Entity;
    }
}