namespace Application.Models.Payment;

public class VerifyPaymentRequest
{
    public string PartnerCode { get; }
    public string OrderId { get; }
    public string RequestId { get; }
    public string Amount { get; }
    public string OrderInfo { get; } 
    public string OrderType{ get; }
    public string TransId{ get; } 
    public string ResponseTime{ get; } 
    public int ResultCode{ get; }
    public string Message{ get; }
    public string PayType{ get; } 
    public string Signature { get; } 
    public string RequestType { get; }
    public string ExtraData { get; }

    public VerifyPaymentRequest(string partnerCode, string orderId, string requestId, string amount, string orderInfo, string orderType, string transId, string responseTime, int resultCode, string message, string payType, string signature, string requestType, string extraData)
    {
        PartnerCode = partnerCode;
        OrderId = orderId;
        RequestId = requestId;
        Amount = amount;
        OrderInfo = orderInfo;
        OrderType = orderType;
        TransId = transId;
        ResponseTime = responseTime;
        ResultCode = resultCode;
        Message = message;
        PayType = payType;
        Signature = signature;
        RequestType = requestType;
        ExtraData = extraData;
    }

    public VerifyPaymentRequest()
    {
        
    }
}
