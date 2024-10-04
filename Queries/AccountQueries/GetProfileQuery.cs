using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Queries.AccountQueries;

public class GetProfileQuery: IRequest<GetProfileQueryResponse>
{
    public Guid Id { get; set; }
}

public class GetProfileQueryResponse
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string ProfilePicture { get; set; }
    public AccountType AccountType { get; set; }
    public int TotalMemberships { get; set; }
    public int TotalTrip { get; set; }
}

public class GetMeQueryHandler: IRequestHandler<GetProfileQuery, GetProfileQueryResponse>
{
    private readonly TransportationContext _context;

    public GetMeQueryHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<GetProfileQueryResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var account = await _context
            .Accounts
            .FirstAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        
        var totalMemberhips = await _context
            .AccountMemberships
            .CountAsync(x => x.AccountId == request.Id, cancellationToken: cancellationToken);

        var totalTrips = await _context
            .TripHeaders
            .CountAsync(x => x.AccountId == request.Id, cancellationToken: cancellationToken);

        return new GetProfileQueryResponse
        {
            Name = account.Name,
            Surname = account.Surname,
            AccountType = account.AccountType,
            ProfilePicture = account.ProfilePicture,
            Username = account.Username,
            TotalMemberships = totalMemberhips,
            TotalTrip = totalTrips
        };
    }
}