using PaymentGateway.Api.Features.PostPayment.Presentation;

namespace PaymentGateway.Api.Features.PostPayment
{
    public interface IPostPaymentHandler
    {
        Task<PostPaymentResponse> Handle(PostPaymentRequest request, CancellationToken cancellationToken = default);
    }
}