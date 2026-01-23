using PaymentGateway.Api.Features.PostPayment.Acquiring;
using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;
using PaymentGateway.Api.Features.PostPayment.Contract;
using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure;
using PaymentGateway.Api.Shared;

namespace PaymentGateway.Api.Features.PostPayment
{
    public class PostPaymentHandler(IPaymentsRepository repository, IAcquiringBankGateway acquiringBankGateway, IDateTimeProvider dateTimeProvider) : IPostPaymentHandler
    {
        public PostPaymentResponse Handle(PostPaymentRequest request)
        {
            var expiryDate = ExpiryDate.From(request.ExpiryMonth, request.ExpiryYear);
            var cardNumber = new CardNumber(request.CardNumber);
            var authorisationRequest = new AuthorisationRequest(cardNumber, expiryDate, request.Currency, request.Amount, request.Cvv);
            var response = acquiringBankGateway.AuthorisePayment(authorisationRequest);
            return new PostPaymentResponse
            {
                Amount = request.Amount,
                CardNumberLastFour = cardNumber.LastFourDigits(),
                Currency = request.Currency,
                ExpiryMonth = request.ExpiryMonth,
                ExpiryYear = request.ExpiryYear,
                Id = response.AuthorisationCode ?? Guid.Empty,
                Status = response.Authorised ? PaymentStatus.Authorized : PaymentStatus.Declined
            };
        }
    }
}