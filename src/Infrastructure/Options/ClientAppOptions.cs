namespace Infrastructure.Options;
[KeyOptions("ClientConfig")]
public class ClientAppOptions
{
    public IReadOnlyCollection<Client>  Clients { get; set; }
}

public class Client
{
    public string Type { get; set; }
    public string Address { get; set; }
    public string ClientId { get; set; }
}
