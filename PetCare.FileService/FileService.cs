using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace PetCare.FileService;

public interface IFileService
{
    Task UploadFile(byte[] fileBytes, string extension, string key);
    Task<byte[]> DownloadFile(string key);
    string GetFileUrl(string key);
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
        _config = config;
        _client = new AmazonS3Client(_config!.SpacesKey, _config.SpacesSecret, s3Config);
    }

    public async Task UploadFile(byte[] fileBytes, string extension, string key)
    {
        var putObjectRequest = new PutObjectRequest()
        {
            BucketName = _config.BucketName,
            Key = $"{key}.{extension}",
            InputStream = new MemoryStream(fileBytes),
            ContentType = "application/octet-stream",
            AutoCloseStream = true,
            CannedACL = S3CannedACL.PublicRead
        };
        await _client.PutObjectAsync(putObjectRequest);
    }

    public async Task<byte[]> DownloadFile(string key)
    {
        var getObjectRequest = new GetObjectRequest()
        {
            BucketName = _config.BucketName,
            Key = key
        };
        var response = await _client.GetObjectAsync(getObjectRequest);
        byte[] fileBytes;
        using (var memoryStream = new MemoryStream())
        {
            await response.ResponseStream.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
        }
        return fileBytes;
    }

    public string GetFileUrl(string key)
    {
        return $"{_config.ServiceUrl}/{_config.BucketName}/{key}";
    }
}