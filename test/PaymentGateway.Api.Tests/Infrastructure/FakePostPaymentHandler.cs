using PaymentGateway.Api.Features.PostPayment;
using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure;
using PaymentGateway.Api.Shared;

namespace PaymentGateway.Api.Tests.Infrastructure
{
    public class FakePostPaymentHandler(bool useValidationFailure) : IPostPaymentHandler
    {
        public static readonly Guid DefaultAuthorisationCode = Guid.Parse("c741d457-6d55-4ed2-afdb-f348fcd42e8a");
        
        public Task<PostPaymentResponse> Handle(PostPaymentRequest request, CancellationToken cancellationToken = default)
        {
            if (useValidationFailure)
            {
                return Task.FromResult(request.ToRejectedResponse());
            }
            
            return Task.FromResult(new PostPaymentResponse
            {
                Amount = request.Amount,
                CardNumberLastFour = 1234,
                Currency = request.Currency,
                ExpiryMonth = request.ExpiryMonth,
                ExpiryYear = request.ExpiryYear,
                Id = DefaultAuthorisationCode,
                Status = PaymentStatus.Authorized
            });
        }
    }
}