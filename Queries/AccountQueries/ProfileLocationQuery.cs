using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Transferciniz.API.Queries.AccountQueries;

public class ProfileLocationQuery : IRequest<List<ProfileLocationQueryResponse>>
{
    public string Search { get; set; }
}

public class ProfileLocationQueryResponse
{
    public string ProfilePicture { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class ProfileLocationQueryHanlder : IRequestHandler<ProfileLocationQuery, List<ProfileLocationQueryResponse>>
{
    private readonly TransportationContext _context;

    public ProfileLocationQueryHanlder(TransportationContext context)
    {
        _context = context;
    }

    public async Task<List<ProfileLocationQueryResponse>> Handle(ProfileLocationQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Accounts.Where(x =>
                x.Name.ToLower().Contains(request.Search.ToLower()) ||
                x.Surname.ToLower().Contains(request.Search.ToLower()))
            .Select(x => new ProfileLocationQueryResponse
            {
                Id = x.Id,
                ProfilePicture = x.ProfilePicture,
                Surname = x.Surname,
                Name = x.Name,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            })
            .ToListAsync(cancellationToken: cancellationToken);
        
    }
}