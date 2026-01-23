using PaymentGateway.Api.Features.PostPayment.Presentation;

namespace PaymentGateway.Api.Features.PostPayment.Contract
{
    public interface IPostPaymentHandler
    {
        PostPaymentResponse Handle(PostPaymentRequest request);
    }
}