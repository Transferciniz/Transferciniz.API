using MediatR;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
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
    private readonly ILogger<UploadProfilePictureCommandHandler> _logger;

    public UploadProfilePictureCommandHandler(IUserSession session, IS3Service s3Service, TransportationContext context, IConfiguration configuration, ILogger<UploadProfilePictureCommandHandler> logger)
    {
        _session = session;
        _s3Service = s3Service;
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<UploadProfilePictureCommandResponse> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
    {
       
        var url = await _s3Service.UploadFileToSpacesAsync(await ProcessImage(request.File));
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

    private static async Task<IFormFile> ProcessImage(IFormFile file, int maxWidth = 500, int maxHeight = 500, int quality = 60)
    {
        // MemoryStream oluştur ve byte dizisini buraya yaz
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;
        
        // ImageSharp ile resmi açın
        using var image = await Image.LoadAsync(stream);

        // Orantılı yeniden boyutlandırma için yeni genişlik ve yükseklik hesaplama
        int originalWidth = image.Width;
        int originalHeight = image.Height;

        double aspectRatio = (double)originalWidth / originalHeight;

        int newWidth = originalWidth;
        int newHeight = originalHeight;

        if (originalWidth > maxWidth || originalHeight > maxHeight)
        {
            if (aspectRatio > 1) // Yatay resim
            {
                newWidth = maxWidth;
                newHeight = (int)(maxWidth / aspectRatio);
            }
            else // Dikey resim veya kare
            {
                newHeight = maxHeight;
                newWidth = (int)(maxHeight * aspectRatio);
            }
        }

        // Boyutlandırma (en-boy oranını koruyarak)
        image.Mutate(x => x.Resize(newWidth, newHeight));

        // Resmi sıkıştırma (JPEG formatında ve belirli kalite ile)
        var encoder = new JpegEncoder()
        {
            Quality = quality // JPEG kalitesi (1-100 arası)
        };

        // Sıkıştırılmış resmi belleğe yaz
        using var outputStream = new MemoryStream();
        await image.SaveAsync(outputStream, encoder);
        outputStream.Position = 0;
        return new FormFile(outputStream, 0, outputStream.Length, "file", file.Name)
        {
            Headers = new HeaderDictionary(),
            ContentType = file.ContentType
        };

    }
    
  
}