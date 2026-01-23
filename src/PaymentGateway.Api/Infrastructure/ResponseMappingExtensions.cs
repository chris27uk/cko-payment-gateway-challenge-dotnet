using PaymentGateway.Api.Features.PostPayment.Acquiring;
using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;
using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Shared;

namespace PaymentGateway.Api.Infrastructure
{
    public static class ResponseMappingExtensions
    {
        public static PostPaymentResponse ToSuccessfulResponse(this PostPaymentRequest request, AuthorisationResponse authorisationResponse)
        {
            var cardNumber = new CardNumber(request.CardNumber);
            var isAuthorised  = authorisationResponse.Authorised;
            var authorisationCode = authorisationResponse.AuthorisationCode;
            return new PostPaymentResponse
            {
                ExpiryMonth = request.ExpiryMonth,
                Status = isAuthorised ? PaymentStatus.Authorized : PaymentStatus.Declined,
                ExpiryYear = request.ExpiryYear,
                Amount = request.Amount,
                CardNumberLastFour = cardNumber.LastFourDigits(),
                Currency = request.Currency,
                Id = authorisationCode ?? Guid.Empty
            };
        }
        
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