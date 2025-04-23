namespace Infrastructure.Configurations;

public class KeycloakConfiguration
{
    public string AddressUrl { get; set; }
    public string Realm { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string GrantType { get; set; }
}
