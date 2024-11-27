using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Helpers;
using Transferciniz.API.Services;

namespace Transferciniz.API.Commands.AccountCommands;

public class UploadProfilePictureCommand: IRequest<UploadProfilePictureCommandResponse>
{
    public string File { get; set; }
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
        var url = await _s3Service.UploadFileToSpacesAsync(ConvertBase64ToJpg(request.File, $"{Guid.NewGuid()}.jpg"));
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
    
    public static IFormFile ConvertBase64ToJpg(string base64String, string fileName)
    {
        // Base64 header'ını kaldır (örneğin: data:image/jpeg;base64,)
        if (base64String.StartsWith("data:image/jpeg;base64,"))
        {
            base64String = base64String.Substring("data:image/jpeg;base64,".Length);
        }
        else if (base64String.StartsWith("data:image/png;base64,"))
        {
            base64String = base64String.Substring("data:image/png;base64,".Length);
        }

        // Base64 string'i byte dizisine çevir
        byte[] imageBytes = Convert.FromBase64String(base64String);

        // MemoryStream oluştur ve byte dizisini buraya yaz
        var stream = new MemoryStream(imageBytes);

        // IFormFile oluştur
        var formFile = new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg" // veya ilgili içerik tipi (örn. "image/png")
        };

        return formFile;
    }
}