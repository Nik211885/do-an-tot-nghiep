using Application.Interfaces.UploadFile;
using CloudinaryDotNet;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.UploadFile;

public class CloudinaryUploadFileServices(IOptions<CloudinaryUploadFileConfiguration> cloudinaryConfiguration,
    Cloudinary cloudinary)
    : IUploadFileServices
{
    private readonly Cloudinary _cloudinary = cloudinary;
    private readonly CloudinaryUploadFileConfiguration
        _cloudinaryUploadFileConfiguration = cloudinaryConfiguration.Value ??
                                             throw new Exception("You can't configure Cloudinary upload configuration with key [UploadFile:Cloudinary]");
    public string GetUrlUploadFileBySignature()
    {
        var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var parameters = new SortedDictionary<string, object>
        {
            {"folder", _cloudinaryUploadFileConfiguration.UploadFolder },
            {"timestamp", timeStamp},
        };
        var stringSign = string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        var signature = _cloudinary.Api.SignParameters(parameters);
        var urlWithSignature =
            string.Concat(_cloudinaryUploadFileConfiguration.UploadFolder, 
                string.Format("/{0}/image/upload?api_key={1}&{2}&signature={3}",
                    _cloudinaryUploadFileConfiguration.CloudName, 
                    _cloudinaryUploadFileConfiguration.ApiKey, 
                    stringSign, 
                    signature));
        return urlWithSignature;
    }
}
