using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Queries.VehicleFiltersQueries;

public class GetVehicleFiltersQuery: IRequest<GetVehicleFiltersQueryResponse>
{
    
}

public class GetVehicleFiltersQueryResponse
{
    public List<ExtraService> ExtraServices { get; set; }
    public List<VehicleSegment> VehicleSegments { get; set; }
    public List<VehicleType> VehicleTypes { get; set; }
}

public class GetVehicleFiltersQueryHandler: IRequestHandler<GetVehicleFiltersQuery, GetVehicleFiltersQueryResponse>
{
    private readonly TransportationContext _context;

    public GetVehicleFiltersQueryHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<GetVehicleFiltersQueryResponse> Handle(GetVehicleFiltersQuery request, CancellationToken cancellationToken)
    {
        var extraServices = await _context.ExtraServices.ToListAsync(cancellationToken: cancellationToken);
        var vehicleSegments = await _context.VehicleSegments.ToListAsync(cancellationToken: cancellationToken);
        var vehicleTypes = await _context.VehicleTypes.ToListAsync(cancellationToken: cancellationToken);
        return new GetVehicleFiltersQueryResponse
        {
            ExtraServices = extraServices,
            VehicleSegments = vehicleSegments,
            VehicleTypes = vehicleTypes
        };
    }
}