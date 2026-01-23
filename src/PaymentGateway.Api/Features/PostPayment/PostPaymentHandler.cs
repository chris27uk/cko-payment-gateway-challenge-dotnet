using PaymentGateway.Api.Features.PostPayment.Acquiring;
using PaymentGateway.Api.Features.PostPayment.Contract;
using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure;
using PaymentGateway.Api.Shared;

namespace PaymentGateway.Api.Features.PostPayment
{
    public class PostPaymentHandler(IPaymentsRepository repository, IAcquiringBankGateway acquiringBankGateway) : IPostPaymentHandler
    {
        public PostPaymentResponse Handle(PostPaymentRequest request)
        {
            var response = acquiringBankGateway.AuthorisePayment(new AuthorisationRequest());
            return new PostPaymentResponse
            {
                Amount = request.Amount,
                CardNumberLastFour = int.Parse(request.CardNumber[^4..]),
                Currency = request.Currency,
                ExpiryMonth = request.ExpiryMonth,
                ExpiryYear = request.ExpiryYear,
                Id = response.AuthorisationCode ?? Guid.Empty,
                Status = response.Authorised ? PaymentStatus.Authorized : PaymentStatus.Declined
            };
        }
    }
}