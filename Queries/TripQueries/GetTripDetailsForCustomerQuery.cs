using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Transferciniz.API.DTOs;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Queries.TripQueries;

public class GetTripDetailsForCustomerQuery: IRequest<TripDto>
{
    public Guid TripHeaderId { get; set; }
}

public class GetTripDetailsForCustomerQueryHandler: IRequestHandler<GetTripDetailsForCustomerQuery, TripDto>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _session;

    public GetTripDetailsForCustomerQueryHandler(TransportationContext context, IUserSession session)
    {
        _context = context;
        _session = session;
    }

    public async Task<TripDto> Handle(GetTripDetailsForCustomerQuery request, CancellationToken cancellationToken)
    {
        var trip = await _context.Trips
            .AsNoTracking()
            .Where(x => x.TripHeaderId == request.TripHeaderId)
            .Include(x => x.AccountVehicle)
            .ThenInclude(x => x.Vehicle)
            .ThenInclude(x => x.VehicleBrand)

            .Include(x => x.AccountVehicle)
            .ThenInclude(x => x.Vehicle)
            .ThenInclude(x => x.VehicleModel)

            .Include(x => x.WayPoints)
            .ThenInclude(x => x.WayPointUsers)
            .ThenInclude(x => x.Account)
            .FirstAsync(cancellationToken: cancellationToken);
        var driver = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == trip.AccountVehicle.DriverId, cancellationToken: cancellationToken);
        var response =  driver is null ? trip.ToDto(): trip.ToDto(driver);

        response.WaypointStatus = trip.WayPoints.First(x => x.WayPointUsers.Any(x => x.AccountId == _session.Id)).Status;
        return response;

    }
}