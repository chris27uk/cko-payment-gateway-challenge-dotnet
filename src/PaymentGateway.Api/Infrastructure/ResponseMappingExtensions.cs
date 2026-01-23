using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Shared;

namespace PaymentGateway.Api.Infrastructure
{
    public static class ResponseMappingExtensions
    {
        public static PostPaymentResponse ToRejectedResponse(this PostPaymentRequest? request)
        {
            if (request == null)
            {
                return new PostPaymentResponse { Status = PaymentStatus.Rejected, Id = Guid.Empty };
            }
            
            return new PostPaymentResponse
            {
                Status = PaymentStatus.Rejected,
                Amount = request.Amount,
                ExpiryMonth = request.ExpiryMonth,
                ExpiryYear = request.ExpiryYear,
                CardNumberLastFour = 0,
                Currency = request.Currency,
                Id = Guid.Empty
            };
        }
    }
}