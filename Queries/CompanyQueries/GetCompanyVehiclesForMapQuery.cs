using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Services;

namespace Transferciniz.API.Queries.CompanyQueries;

public class GetCompanyVehiclesForMapQuery: IRequest<List<MapTrackingResponse>>
{
    public Guid CompanyId { get; set; }
    public bool IncludeDrivers { get; set; }
}


public class MapTrackingResponse
{
    public string Name { get; set; }
    public string Description { get; set; }
    public MapEntityType Type { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Picture { get; set; }
}

public enum MapEntityType
{
    Person,
    Vehicle
}

public class GetCompanyVehiclesForMapHandler: IRequestHandler<GetCompanyVehiclesForMapQuery, List<MapTrackingResponse>>
{
    private readonly IUserSession _userSession;
    private readonly TransportationContext _context;

    public GetCompanyVehiclesForMapHandler(IUserSession userSession, TransportationContext context)
    {
        _userSession = userSession;
        _context = context;
    }

    public async Task<List<MapTrackingResponse>> Handle(GetCompanyVehiclesForMapQuery request, CancellationToken cancellationToken)
    {
        return await _context.CompanyVehicles
            .Where(x => x.CompanyId == (Guid.Empty == request.CompanyId ? _userSession.CompanyId : request.CompanyId))
            .Select(x => new MapTrackingResponse
            {
                Description = $"{x.Plate} Plakalı Araç",
                Name = x.Plate,
                Type = MapEntityType.Vehicle,
                Picture = string.Empty,
                Latitude = x.Location.Coordinate.X,
                Longitude = x.Location.Coordinate.Y
            })
            .ToListAsync(cancellationToken: cancellationToken);
    }
    
}