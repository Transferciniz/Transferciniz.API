using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Helpers;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AccountCommands;

public class UploadProfilePictureCommand: IRequest<UploadProfilePictureCommandResponse>
{
    public IFormFile File { get; set; }
}

public class UploadProfilePictureCommandResponse
{
    public string Token { get; set; }
}

public class UploadProfilePictureCommandHandler : IRequestHandler<UploadProfilePictureCommand, UploadProfilePictureCommandResponse>
{
    private readonly IUserSession _session;
    private readonly IS3Service _s3Service;
    private readonly TransportationContext _context;
    private readonly IConfiguration _configuration;

    public UploadProfilePictureCommandHandler(IUserSession session, IS3Service s3Service, TransportationContext context, IConfiguration configuration)
    {
        _session = session;
        _s3Service = s3Service;
        _context = context;
        _configuration = configuration;
    }

    public async Task<UploadProfilePictureCommandResponse> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var url = await _s3Service.UploadFileToSpacesAsync(request.File);
        var user = await _context.Accounts.Where(x => x.Id == _session.Id).FirstAsync(cancellationToken: cancellationToken);
        var session = await _context.Sessions.FirstAsync(x => x.AccountId == _session.Id, cancellationToken: cancellationToken);
        user.ProfilePicture = url;
        
        _context.Accounts.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return new UploadProfilePictureCommandResponse
        {
            Token = user.GenerateToken(_configuration, session.Id)
        };
    }
}