namespace Infrastructure.Options;
[KeyOptions("RabbitMq")]
public class RabbitMqOptions
{
    public string Host { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
