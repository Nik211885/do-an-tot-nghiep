using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Application.Interfaces.Payment;
using Application.Models.Payment;
using Core.Exception;
using Infrastructure.Helper;
using Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Payment;

public class MomoGatewayPayment(
    ILogger<MomoGatewayPayment> logger,
    IOptions<MomoConfigOptions> momoConfigOptions,
    IHttpClientFactory clientFactory)
    : IPayment
{
    private readonly ILogger<MomoGatewayPayment> _logger = logger;
    private readonly MomoConfigOptions _momoConfigOptions = momoConfigOptions.Value;
    private readonly HttpClient _client = clientFactory
        .CreateClient(HttpClientKeyFactory.BankMomoKey);

    public async Task<PaymentResponse> CreatePaymentRequestAsync(PaymentRequest paymentRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var requestId = DateTime.UtcNow.Ticks.ToString();
            var orderId = paymentRequest.OrderId;
            var orderInfo = paymentRequest.OrderInfo;
            // Fix 1: Viet nam use for 00000 don't use  decimal like 6.6999$ ;)))
            var amount = ((long)paymentRequest.Amount).ToString();
            string extraData = paymentRequest.ExtraData;
            
            string requestType = paymentRequest.PaymentMethod switch
            {
                PaymentMethod.Wallet => "captureWallet",
                PaymentMethod.Atm => "payWithATM",
                PaymentMethod.Credit => "payWithCC",
                PaymentMethod.Qr => "captureWallet",
                _ => throw new BadRequestException($"Can't not support payment method {paymentRequest.PaymentMethod}")
            };
            
            var rawHash =
                $"accessKey={_momoConfigOptions.AccessKey}" +
                $"&amount={amount}" +
                $"&extraData={extraData}" +
                $"&ipnUrl={_momoConfigOptions.IpnUrl}" +
                $"&orderId={orderId}" +
                $"&orderInfo={orderInfo}" +
                $"&partnerCode={_momoConfigOptions.PartnerCode}" +
                $"&redirectUrl={_momoConfigOptions.ReturnUrl}" +  
                $"&requestId={requestId}" +
                $"&requestType={requestType}";

            _logger.LogInformation("Raw hash string: {RawHash}", rawHash);
            var signature = ComputeHmacSha256(rawHash, _momoConfigOptions.SecretKey);
            _logger.LogInformation("Generated signature: {Signature}", signature);

            var requestBody = new
            {
                accessKey = _momoConfigOptions.AccessKey,
                partnerCode = _momoConfigOptions.PartnerCode,
                requestType = requestType,
                redirectUrl = _momoConfigOptions.ReturnUrl, 
                ipnUrl = _momoConfigOptions.IpnUrl,
                orderId = orderId,
                amount = amount,
                orderInfo = orderInfo,
                requestId = requestId,
                extraData = extraData,
                signature = signature
            };

            _logger.LogInformation("Request body: {RequestBody}", JsonSerializer.Serialize(requestBody));

            var response = await _client.PostAsync(_momoConfigOptions.PaymentUrl, requestBody.GetStringContent(), cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation("MoMo response: {Response}", responseContent);

            using var doc = JsonDocument.Parse(responseContent);
            var root = doc.RootElement;

            if (root.GetProperty("resultCode").GetInt32() == 0)
            {
                return new PaymentResponse
                (
                    success: true,
                    paymentUrl: root.GetProperty("payUrl").GetString(),
                    qrCodeUrl: root.TryGetProperty("qrCodeUrl", out var qr) ? qr.GetString() : null,
                    deeplink: root.TryGetProperty("deeplink", out var dl) ? dl.GetString() : null,
                    applink: root.TryGetProperty("applink", out var al) ? al.GetString() : null,
                    message: null
                );
            }

            throw new Exception(
                $"MoMo payment creation failed: {root.GetProperty("message")}. ResultCode: {root.GetProperty("resultCode")}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment request");
            throw;
        }
    }

    public bool VerifyPaymentResponse(VerifyPaymentRequest verifyPaymentRequest)
    {
        try
        {
            var rawHash = $"accessKey={_momoConfigOptions.AccessKey}" +
                          $"&amount={verifyPaymentRequest.Amount}" +
                          $"&extraData={verifyPaymentRequest.ExtraData}" +
                          $"&message={verifyPaymentRequest.Message}" +
                          $"&orderId={verifyPaymentRequest.OrderId}" +
                          $"&orderInfo={verifyPaymentRequest.OrderInfo}" +
                          $"&orderType={verifyPaymentRequest.OrderType}" +
                          $"&partnerCode={verifyPaymentRequest.PartnerCode}" +
                          $"&payType={verifyPaymentRequest.PayType}" +
                          $"&requestId={verifyPaymentRequest.RequestId}" +
                          $"&responseTime={verifyPaymentRequest.ResponseTime}" +
                          $"&resultCode={verifyPaymentRequest.ResultCode}" +
                          $"&transId={verifyPaymentRequest.TransId}";
            
            _logger.LogInformation("Raw hash string for verification: {RawHash}", rawHash);
            var expectedSignature = ComputeHmacSha256(rawHash, _momoConfigOptions.SecretKey);
            _logger.LogInformation("Expected signature: {ExpectedSignature}", expectedSignature);
            _logger.LogInformation("Received signature: {ReceivedSignature}", verifyPaymentRequest.Signature);
            
            if (verifyPaymentRequest.ResultCode == 0)
            {
                var isSignatureValid = string.Equals(verifyPaymentRequest.Signature, expectedSignature, StringComparison.OrdinalIgnoreCase);
                if (!isSignatureValid)
                {
                    _logger.LogWarning("Payment successful but signature mismatch - this could indicate a security issue");
                }
                return isSignatureValid;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying payment request");
            throw;
        }
    }

    private string ComputeHmacSha256(string message, string secretKey)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var messageBytes = Encoding.UTF8.GetBytes(message);
        using var hmac = new HMACSHA256(keyBytes);
        var hashBytes = hmac.ComputeHash(messageBytes);
        return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
    }
}
