using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.StaticFiles;

namespace Transferciniz.API.Services;

public interface IS3Service
{
    Task<string> UploadFileToSpacesAsync(IFormFile file);
}

public class S3Service : IS3Service
{
    private static readonly string accessKey = "DO00BXHFV9YUNC78DXZT";
    private static readonly string secretKey = "3Gs1fYlsHv5PuJeLYvnxS1VdqOgdn1quO5I/xemzOWM";
    private static readonly string spaceName = "transferciniz-bucket";
    private static readonly string region = "nyc3"; // DigitalOcean Spaces'teki bölgeyi belirtin
    private static readonly string serviceUrl = $"https://{region}.digitaloceanspaces.com";
    private static readonly string bucketName = "transferciniz-bucket";

    private AmazonS3Client _client;

    public S3Service()
    {
        _client = new AmazonS3Client(accessKey, secretKey, new AmazonS3Config
        {
            ServiceURL = serviceUrl,
            ForcePathStyle = true // DigitalOcean Spaces ile uyumlu olması için zorunlu
        });
    }
    
    public async Task<string> UploadFileToSpacesAsync(IFormFile file)
    {
        // Geçici bir klasöre kaydetme
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var tempFilePath = Path.Combine(Path.GetTempPath(), fileName);

        try
        {
            // Dosyayı geçici dizine kaydetme
            await using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // MIME türünü çözümleme
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(tempFilePath, out contentType))
            {
                contentType = "application/octet-stream"; // Varsayılan content type
            }
            

            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileName, // Spaces'teki dosya adı
                FilePath = tempFilePath, // Geçici dosya yolu
                ContentType = contentType, // MIME türü
                CannedACL = S3CannedACL.PublicRead 
            };

            var response = await _client.PutObjectAsync(putRequest);

            // Başarılı ise dosyanın URL'sini döndür
            var fileUrl = $"{serviceUrl}/{bucketName}/{fileName}";
            return fileUrl;
        }
        catch (Exception ex)
        {
            // Hata durumunda loglama ve exception fırlatma
            Console.WriteLine($"Dosya yüklenirken hata oluştu: {ex.Message}");
            throw;
        }
        finally
        {
            // Geçici dosyayı sil
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }
}