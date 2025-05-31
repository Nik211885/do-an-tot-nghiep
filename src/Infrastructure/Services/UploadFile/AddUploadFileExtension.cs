using Application.Interfaces.UploadFile;
using CloudinaryDotNet;
using Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.UploadFile;
/// <summary>
///     
/// </summary>
public static class AddUploadFileExtension
{
    /// <summary>
    ///     Add Cloudinary cloud make upload file with cloudinary account
    ///     and upload file into cloudinary services 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static IServiceCollection AddUploadFileWithCloudinary(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var cloudinaryUploadFileConfiguration = serviceProvider.GetRequiredService<IOptions<CloudinaryUploadFileOptions>>().Value
            ?? throw new Exception("You can't configure Cloudinary upload configuration with key [UploadFile:Cloudinary]");
        var account = new Account()
        {
            Cloud = cloudinaryUploadFileConfiguration.CloudName,
            ApiKey = cloudinaryUploadFileConfiguration.ApiKey,
            ApiSecret = cloudinaryUploadFileConfiguration.ApiSecret,
        };
        var cloudinary = new Cloudinary(account);
        services.AddSingleton(cloudinary);
        services.AddSingleton<IUploadFileServices, CloudinaryUploadFileServices>();
        return services;
    }
}
