using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Queries.VehicleQueries;

public class GetVehiclesQuery: IRequest<List<GetVehiclesQueryResponse>>
{
}

public class GetVehiclesQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Photo { get; set; }
    public int Capacity { get; set; }
}

public class GetVehiclesQueryHandler: IRequestHandler<GetVehiclesQuery, List<GetVehiclesQueryResponse>>
{
    private readonly TransportationContext _context;

    public GetVehiclesQueryHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<List<GetVehiclesQueryResponse>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Vehicles
            .Include(x => x.VehicleModel)
            .Include(x => x.VehicleBrand)
            .Select(x => new GetVehiclesQueryResponse
            {
                Id = x.Id,
                Capacity = x.VehicleModel.Capacity,
                Name = $"{x.VehicleBrand.Name} {x.VehicleModel.Name}",
                Photo = x.VehicleModel.Photo
            })
            .ToListAsync(cancellationToken: cancellationToken);
    }
}