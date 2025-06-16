namespace Infrastructure.Options;

[KeyOptions("UploadFile:Cloudinary")]
public class CloudinaryUploadFileOptions
{
    public string UrlUpload { get; set; }
    public string CloudName { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
    public string UploadFolder { get; set; }
}
