using PaymentGateway.Api.Features.PostPayment.Acquiring;
using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;
using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure;
using PaymentGateway.Api.Infrastructure.Persistence;

namespace PaymentGateway.Api.Features.PostPayment
{
    public class PostPaymentHandler(
        IPaymentsRepository repository, 
        IAcquiringBankGateway acquiringBankGateway, 
        IDateTimeProvider dateTimeProvider,
        IObservabilityProbe observabilityProbe) : IPostPaymentHandler
    {
        public PostPaymentResponse Handle(PostPaymentRequest request)
        {
            var authorisationRequest = new AuthorisationRequest(
                request.CardNumber, 
                ExpiryDate.From(request.ExpiryMonth, request.ExpiryYear), 
                request.Currency, 
                request.Amount, 
                request.Cvv);
            
            if (!authorisationRequest.Validate(dateTimeProvider, observabilityProbe))
            {
                return request.ToRejectedResponse();
            }
            
            var authorisationResponse = acquiringBankGateway.AuthorisePayment(authorisationRequest);
            var paymentResponse = request.ToSuccessfulResponse(authorisationResponse);
            repository.Add(paymentResponse);
            return paymentResponse;
        }
    }
}