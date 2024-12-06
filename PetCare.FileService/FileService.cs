using Amazon;
using Amazon.S3;

namespace PetCare.FileService;

public interface IFileService
{
    
}

public class FileService : IFileService 
{
    private readonly FileSystemConfig _config;
    private readonly AmazonS3Client _client;

    public FileService(FileSystemConfig config)
    {
        var s3Config = new AmazonS3Config();
        _client = new AmazonS3Client(_config!.SpacesKey, _config.SpacesSecret, s3Config);
        _config = config;
    }
}