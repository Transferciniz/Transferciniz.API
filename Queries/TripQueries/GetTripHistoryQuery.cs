using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.DTOs;
using Transferciniz.API.Entities;
using Transferciniz.API.Helpers;
using Transferciniz.API.Services;

namespace Transferciniz.API.Queries.TripQueries;

public class GetTripHistoryQuery: IRequest<PagingResult<List<TripHeaderDto>>>
{
    public int Skip { get; set; }
    public int Take { get; set; }
}

public class GetTripHistoryQueryHandler: IRequestHandler<GetTripHistoryQuery, PagingResult<List<TripHeaderDto>>>
{
    private readonly IUserSession _userSession;
    private readonly TransportationContext _context;

    public GetTripHistoryQueryHandler(IUserSession userSession, TransportationContext context)
    {
        _userSession = userSession;
        _context = context;
    }

    public async Task<PagingResult<List<TripHeaderDto>>> Handle(GetTripHistoryQuery request, CancellationToken cancellationToken)
    {
        switch (_userSession.AccountType)
        {
            case AccountType.EnterpriseCustomerCompany:
                return await GetCustomerTripHistory(request,cancellationToken);
            case AccountType.EnterpriseTransporterCompany:
                return await GetCompanyTripHistory(request,cancellationToken);
            case AccountType.Customer:
                return await GetCustomerTripHistory(request,cancellationToken);
            case AccountType.Driver:
                return await GetCustomerTripHistory(request,cancellationToken);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task<PagingResult<List<TripHeaderDto>>> GetCustomerTripHistory(GetTripHistoryQuery request,CancellationToken cancellationToken)
    {
        var waypointIds = await _context.WayPointUsers
            .Where(x => x.AccountId == _userSession.Id)
            .Select(x => x.WayPointId)
            .ToListAsync(cancellationToken: cancellationToken);

        var tripIds = (await _context.WayPoints
                .Where(x => waypointIds.Contains(x.Id))
                .Select(x => x.TripId)
                .ToListAsync(cancellationToken: cancellationToken))
            .Distinct()
            .ToList();

        var tripHeaderIds = (await _context.Trips
                .Where(x => tripIds.Contains(x.Id) && x.Status == TripStatus.Finished)
                .Select(x => x.TripHeaderId)
                .ToListAsync(cancellationToken: cancellationToken))
            .Distinct()
            .ToList();

        var tripHeaders = await _context.TripHeaders
            .Include(x => x.Trips)
            .Where(x => tripHeaderIds.Contains(x.Id) && x.Status == TripStatus.Finished)
            .OrderByDescending(x => x.StartDate)
            .Skip(request.Skip)
            .Take(request.Take)
            .ToListAsync(cancellationToken: cancellationToken);

        var totalCount = await _context.TripHeaders
            .Include(x => x.Trips)
            .Where(x => tripHeaderIds.Contains(x.Id))
            .CountAsync(cancellationToken: cancellationToken);
        var data =  tripHeaders.Select(x => x.ToDto()).ToList();

        return new PagingResult<List<TripHeaderDto>>
        {
            Data = data,
            TotalCount = totalCount
        };

    }
    
    private async Task<PagingResult<List<TripHeaderDto>>> GetCompanyTripHistory(GetTripHistoryQuery request,CancellationToken cancellationToken)
    {
        var companyVehicles = await _context.AccountVehicles.Where(x => x.AccountId == _userSession.Id)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken: cancellationToken);
        var tripHeaderIds = (await _context.Trips
            .Where(x => companyVehicles.Contains(x.AccountVehicleId) && x.Status == TripStatus.Finished)
            .Select(x => x.TripHeaderId)
            .ToListAsync(cancellationToken: cancellationToken)).Distinct().ToList();
        
        var data = await _context.TripHeaders
            .Include(x => x.Trips)
            .Where(x => tripHeaderIds.Contains(x.Id) && x.Status == TripStatus.Finished)
            .OrderByDescending(x => x.StartDate)
            .Skip(request.Skip)
            .Take(request.Take)
            .ToListAsync(cancellationToken: cancellationToken);
        
        var totalCount = await _context.TripHeaders
            .Include(x => x.Trips)
            .Where(x => tripHeaderIds.Contains(x.Id) && x.Status == TripStatus.Finished)
            .CountAsync(cancellationToken: cancellationToken);
        
        return new PagingResult<List<TripHeaderDto>>
        {
            Data = data.Select(x => x.ToDto()).ToList(),
            TotalCount = totalCount
        };
    }
}