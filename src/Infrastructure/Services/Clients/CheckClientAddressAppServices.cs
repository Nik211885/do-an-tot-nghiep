using Application.Interfaces.Clients;
using Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Clients;

public class CheckClientAddressAppServices(IOptions<ClientAppOptions> clientAppOptions) : ICheckClientAddressAppServices
{
    private readonly ClientAppOptions _clientAppOptions = clientAppOptions.Value;

    public bool IsClientAddress(string address)
    {
        if (!Uri.TryCreate(address, UriKind.Absolute, out var inputUri))
            return false;
        return _clientAppOptions.Clients.Any(client =>
        {
            if (!Uri.TryCreate(client.Address, UriKind.Absolute, out var clientUri))
                return false;
            
            return string.Equals(inputUri.Host, clientUri.Host, StringComparison.OrdinalIgnoreCase)
                   && inputUri.Port == clientUri.Port
                   && string.Equals(inputUri.Scheme, clientUri.Scheme, StringComparison.OrdinalIgnoreCase);
        });
    }
}
