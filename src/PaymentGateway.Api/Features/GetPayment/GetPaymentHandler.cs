using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure;

namespace PaymentGateway.Api.Features.GetPayment
{
    public class GetPaymentHandler(IPaymentsRepository paymentRepository)
    {
        public PostPaymentResponse? Handle(Guid paymentId) => paymentRepository.Get(paymentId);
    }
}