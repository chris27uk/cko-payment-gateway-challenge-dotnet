using PaymentGateway.Api.Features.GetPayment.Presentation;
using PaymentGateway.Api.Features.PostPayment.Acquiring;
using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;
using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure.Persistence;
using PaymentGateway.Api.Shared;

namespace PaymentGateway.Api.Infrastructure
{
    public static class ResponseMappingExtensions
    {
        public static PostPaymentResponseStored ToStoredResponse(this PostPaymentRequest request, AuthorisationResponse authorisationResponse)
        {
            var cardNumber = new CardNumber(request.CardNumber);
            var isAuthorised  = authorisationResponse.Authorised;
            var authorisationCode = authorisationResponse.AuthorisationCode;
            return new PostPaymentResponseStored
            {
                ExpiryMonth = request.ExpiryMonth,
                Status = isAuthorised ? PaymentStatus.Authorized : PaymentStatus.Declined,
                ExpiryYear = request.ExpiryYear,
                Amount = request.Amount,
                AuthorisationCode = authorisationCode,
                CardNumberLastFour = cardNumber.LastFourDigits(),
                Currency = request.Currency,
                Id = request.Reference
            };
        }

        public static PostPaymentResponse ToPublicPostResponse(this PostPaymentResponseStored response)
        {
            return new PostPaymentResponse
            {
                ExpiryMonth = response.ExpiryMonth,
                Status = response.Status,
                ExpiryYear = response.ExpiryYear,
                Amount = response.Amount,
                CardNumberLastFour = response.CardNumberLastFour,
                Currency = response.Currency,
                Id = response.Id
            };
        }
        
        public static PostPaymentResponse ToPublicPostResponse(this GetPaymentResponse response)
        {
            return new PostPaymentResponse
            {
                ExpiryMonth = response.ExpiryMonth,
                Status = response.Status,
                ExpiryYear = response.ExpiryYear,
                Amount = response.Amount,
                CardNumberLastFour = response.CardNumberLastFour,
                Currency = response.Currency,
                Id = response.Id
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