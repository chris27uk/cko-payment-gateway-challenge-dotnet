using PaymentGateway.Api.Features.GetPayment.Contract;
using PaymentGateway.Api.Features.GetPayment.Presentation;
using PaymentGateway.Api.Infrastructure;
using PaymentGateway.Api.Infrastructure.Persistence;

namespace PaymentGateway.Api.Features.GetPayment
{
    public class GetPaymentHandler(IPaymentsRepository paymentRepository) : IGetPaymentHandler
    {
        public GetPaymentResponse? Handle(Guid paymentId)
        {
            var postedResponse = paymentRepository.Get(paymentId);

            if (postedResponse == null)
            {
                return null;
            }
            
            return new GetPaymentResponse
            {
                ExpiryMonth = postedResponse.ExpiryMonth,
                ExpiryYear = postedResponse.ExpiryYear,
                Amount = postedResponse.Amount,
                CardNumberLastFour = postedResponse.CardNumberLastFour,
                Currency = postedResponse.Currency,
                Status = postedResponse.Status,
                Id = postedResponse.Id
            };
        }
    }
}