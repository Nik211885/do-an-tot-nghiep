using Application.Interfaces.CQRS;
using Application.Interfaces.UploadFile;

namespace Application.Services.Extend.Query.UploadFile;

public record GetUploadFileUrlWithSignatureQuery : IQuery<string>;

public class GetUploadFileUrlWithSignatureQueryHandler(IUploadFileServices uploadFileServices) : IQueryHandler<GetUploadFileUrlWithSignatureQuery, string>
{
    private readonly IUploadFileServices _uploadFileServices = uploadFileServices;
    public Task<string> Handle(GetUploadFileUrlWithSignatureQuery request, CancellationToken cancellationToken)
    {
        var url = _uploadFileServices.GetUrlUploadFileBySignature();
        return Task.FromResult(url);
    }
}
