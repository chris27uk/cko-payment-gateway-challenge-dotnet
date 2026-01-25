using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;

namespace PaymentGateway.Api.Infrastructure
{
    public interface IObservabilityProbe
    {
        void PaymentDataRejected(string fieldName, CustomerReference customerReference);

        void DuplicatePaymentRequest(CustomerReference reference);
    }
}