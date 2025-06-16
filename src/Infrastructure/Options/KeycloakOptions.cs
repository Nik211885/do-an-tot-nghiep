namespace Infrastructure.Options;

[KeyOptions("KeyCloakAuthentication:BookStoreServer")]
public class KeycloakOptions
{
    public string AddressUrl { get; set; }
    public string Realm { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string GrantType { get; set; }
}
