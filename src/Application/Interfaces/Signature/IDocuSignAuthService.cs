namespace Application.Interfaces.Signature;

public interface IDocuSignAuthService
{
    Task<string> GetAccessTokenAsync();
}
