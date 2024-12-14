using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Shared.Exceptions;

namespace PetCare.FileService;

public static class FileSystemModule
{
    public static void AddFileSystemModule(this IServiceCollection services, IConfiguration builderConfiguration)
    {
        var fileSystemConfiguration = builderConfiguration.GetSection("FileSystemConfig").Get<FileSystemConfig>();
        if (fileSystemConfiguration == null)
        {
            throw new BaseException("File System Configuration is missing");
        }
        services.AddSingleton<FileSystemConfig>(fileSystemConfiguration);
        services.AddScoped<IFileService, FileService>();
    }
}