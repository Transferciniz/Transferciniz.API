using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AuthCommands;

public class UploadProfilePictureCommand: IRequest<UploadProfilePictureCommandResponse>
{
    public IFormFile File { get; set; }
}

public class UploadProfilePictureCommandResponse
{
    public string Url { get; set; }
}

public class UploadProfilePictureCommandHandler : IRequestHandler<UploadProfilePictureCommand, UploadProfilePictureCommandResponse>
{
    private readonly IUserSession _session;
    private readonly IS3Service _s3Service;
    private readonly TransportationContext _context;

    public UploadProfilePictureCommandHandler(IUserSession session, IS3Service s3Service, TransportationContext context)
    {
        _session = session;
        _s3Service = s3Service;
        _context = context;
    }

    public async Task<UploadProfilePictureCommandResponse> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var url = await _s3Service.UploadFileToSpacesAsync(request.File);
        var user = await _context.Users.Where(x => x.Id == _session.Id).FirstAsync(cancellationToken: cancellationToken);
        user.ProfilePicture = url;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return new UploadProfilePictureCommandResponse
        {
            Url = url
        };
    }
}