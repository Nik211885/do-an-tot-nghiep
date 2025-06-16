using Application.Models.Payment;

namespace Application.Interfaces.Payment;

public interface IPayment
{
    Task<PaymentResponse> CreatePaymentRequestAsync(PaymentRequest paymentRequest,
        CancellationToken cancellationToken = default);

    bool VerifyPaymentResponse(VerifyPaymentRequest verifyPaymentRequest);
}
