namespace Application.Models.Payment;

public class PaymentResponse(
    bool success,
    string? paymentUrl,
    string? qrCodeUrl,
    string? deeplink,
    string? applink,
    string? message)
{
    public bool Success { get; set; } = success;
    public string? PaymentUrl { get; set; } = paymentUrl;
    public string? QrCodeUrl { get; set; } = qrCodeUrl;
    public string? Deeplink { get; set; } = deeplink;
    public string? Applink { get; set; } = applink;
    public string? Message { get; set; } = message;
}
