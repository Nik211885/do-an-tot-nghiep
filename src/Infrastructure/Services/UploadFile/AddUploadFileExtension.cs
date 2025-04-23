using CloudinaryDotNet;
using Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.UploadFile;

public static class AddUploadFileExtension
{
    public static IServiceCollection AddUploadFileWithCloudinary(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var cloudinaryUploadFileConfiguration = serviceProvider.GetRequiredService<IOptions<CloudinaryUploadFileConfiguration>>().Value
            ?? throw new Exception("You can't configure Cloudinary upload configuration with key [UploadFile:Cloudinary]");
        var account = new Account()
        {
            Cloud = cloudinaryUploadFileConfiguration.CloudName,
            ApiKey = cloudinaryUploadFileConfiguration.ApiKey,
            ApiSecret = cloudinaryUploadFileConfiguration.ApiSecret,
        };
        var cloudinary = new Cloudinary(account);
        services.AddSingleton(cloudinary);
        return services;
    }
}
