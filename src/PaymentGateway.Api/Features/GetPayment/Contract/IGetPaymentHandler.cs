using PaymentGateway.Api.Features.GetPayment.Presentation;

namespace PaymentGateway.Api.Features.GetPayment.Contract
{
    public interface IGetPaymentHandler
    {
        GetPaymentResponse? Handle(Guid paymentId);
    }
}