using System.Text.Json.Serialization;

namespace Application.Models.Payment;

public class PaymentRequest
{
    public decimal Amount { get; }
    public string OrderId { get; }
    public string OrderInfo { get; }
    public PaymentMethod PaymentMethod { get; }
    public string Lang { get; }
    public string ExtraData { get; }

    public PaymentRequest(decimal amount, string orderId, string orderInfo, 
        PaymentMethod paymentMethod, string? lang, string extraData)
    {
        Amount = amount;
        OrderId = orderId;
        OrderInfo = orderInfo;
        PaymentMethod = paymentMethod;
        Lang = lang ?? "vi";
        ExtraData = extraData;
    }
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentMethod
{
    Wallet,
    Atm,
    Qr,
    Credit,
}
