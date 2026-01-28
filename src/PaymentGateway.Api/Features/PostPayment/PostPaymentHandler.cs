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
        public async Task<PostPaymentResponse> Handle(PostPaymentRequest request, CancellationToken cancellationToken = default)
        {
            var authorisationRequest = new AuthorisationRequest(
                request.CardNumber, 
                ExpiryDate.From(request.ExpiryMonth, request.ExpiryYear), 
                request.Currency, 
                request.Amount, 
                request.Cvv,
                request.Reference);
            
            if (!authorisationRequest.Validate(dateTimeProvider, observabilityProbe))
            {
                return request.ToRejectedResponse();
            }
            
            var authorisationResponse = await acquiringBankGateway.AuthorisePayment(authorisationRequest, cancellationToken);
            var paymentResponse = request.ToStoredResponse(authorisationResponse);
            repository.Add(paymentResponse, cancellationToken);
            return paymentResponse.ToPublicPostResponse();
        }
    }
}