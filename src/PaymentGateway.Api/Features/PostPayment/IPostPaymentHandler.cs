using PaymentGateway.Api.Features.PostPayment.Presentation;

namespace PaymentGateway.Api.Features.PostPayment
{
    public interface IPostPaymentHandler
    {
        PostPaymentResponse Handle(PostPaymentRequest request);
    }
}