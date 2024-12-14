using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace PetCare.FileService;

public interface IFileService
{
    Task UploadFile(byte[] fileBytes, string extension, string key);
}

public class FileService : IFileService 
{
    private readonly FileSystemConfig _config;
    private readonly AmazonS3Client _client;

    public FileService(FileSystemConfig config)
    {
        var s3Config = new AmazonS3Config();
        s3Config.ServiceURL = config.ServiceUrl;
        s3Config.ForcePathStyle = true;
        _client = new AmazonS3Client(_config!.SpacesKey, _config.SpacesSecret, s3Config);
        _config = config;
    }

    public async Task UploadFile(byte[] fileBytes, string extension, string key)
    {
        var putObjectRequest = new PutObjectRequest()
        {
            BucketName = _config.BucketName,
            Key = $"{key}.{extension}",
            InputStream = new MemoryStream(fileBytes),
            ContentType = "application/octet-stream",
            AutoCloseStream = true
        };
        await _client.PutObjectAsync(putObjectRequest);
    }
}