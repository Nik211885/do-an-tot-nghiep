using System.Text.Json.Serialization;

namespace Application.Models.Payment;

public class VerifyPaymentRequest
{
    public string? PartnerCode { get; set; }
    public string? OrderId { get; set;}
    public string? RequestId { get; set;}
    public string? Amount { get; set;}
    public string? OrderInfo { get;set; } 
    public string? OrderType{ get; set;}
    public string? TransId{ get; set;} 
    public string? ResponseTime{ get; set;} 
    public int? ResultCode{ get;set; }
    public string? Message{ get;set; }
    public string? PayType{ get;set; } 
    public string? Signature { get; set;} 
    public string? RequestType { get;set; }
    public string? ExtraData { get;set; }
    
}
