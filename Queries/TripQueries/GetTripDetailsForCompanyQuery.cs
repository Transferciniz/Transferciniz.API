using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.DTOs;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Queries.TripQueries;

public class GetTripDetailsForCompanyQuery: IRequest<List<TripDto>>
{
    public Guid TripHeaderId { get; set; }
}

public class GetTripDetailsForCompanyQueryHandler: IRequestHandler<GetTripDetailsForCompanyQuery, List<TripDto>>
{
    private readonly TransportationContext _context;
    private readonly IUserSession _userSession;

    public GetTripDetailsForCompanyQueryHandler(TransportationContext context, IUserSession userSession)
    {
        _context = context;
        _userSession = userSession;
    }

    public async Task<List<TripDto>> Handle(GetTripDetailsForCompanyQuery request, CancellationToken cancellationToken)
    {
        var trips = await _context.Trips
            .Where(x => x.AccountVehicle.AccountId == _userSession.Id && x.TripHeaderId == request.TripHeaderId)
            .Include(x => x.AccountVehicle)
            .ThenInclude(x => x.Vehicle)
            .ThenInclude(x => x.VehicleBrand)

            .Include(x => x.AccountVehicle)
            .ThenInclude(x => x.Vehicle)
            .ThenInclude(x => x.VehicleModel)

            .Include(x => x.WayPoints)
            .ThenInclude(x => x.WayPointUsers)
            .ThenInclude(x => x.Account)
            .ToListAsync(cancellationToken: cancellationToken);
        return trips.Select(x => x.ToDto()).ToList();
    }
}