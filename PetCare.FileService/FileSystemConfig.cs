namespace PetCare.FileService;

public class FileSystemConfig
{
    public required string ServiceUrl { get; set; }
    public required string SpacesKey { get; set; }
    public required string SpacesSecret { get; set; }
    public required string PathPrefix { get; set; }
    public required string BucketName { get; set; }
}