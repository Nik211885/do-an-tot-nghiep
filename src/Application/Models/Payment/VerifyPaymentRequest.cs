using System.Text.Json.Serialization;

namespace Application.Models.Payment;

public class VerifyPaymentRequest
{
    [JsonPropertyName("partnerCode")]
    public string PartnerCode { get; set; }
    [JsonPropertyName("orderId")]
    public string OrderId { get; set;}
    [JsonPropertyName("requestId")]
    public string RequestId { get; set;}
    [JsonPropertyName("amount")]
    public string Amount { get; set;}
    [JsonPropertyName("orderInfo")]
    public string OrderInfo { get;set; } 
    [JsonPropertyName("orderType")]
    public string OrderType{ get; set;}
    [JsonPropertyName("transId")]
    public string TransId{ get; set;} 
    [JsonPropertyName("responseTime")]
    public string ResponseTime{ get; set;} 
    [JsonPropertyName("resultCode")]
    public int ResultCode{ get;set; }
    [JsonPropertyName("message")]
    public string Message{ get;set; }
    [JsonPropertyName("payType")]
    public string PayType{ get;set; } 
    [JsonPropertyName("signature")]
    public string Signature { get; set;} 
    [JsonPropertyName("requestType")]
    public string RequestType { get;set; }
    [JsonPropertyName("extraData")]
    public string ExtraData { get;set; }
    
}
