namespace PaymentGateway.Api.Infrastructure
{
    public interface IObservabilityProbe
    {
        void PaymentDataRejected(string fieldName, Guid? customerReference);

        void DuplicatePaymentRequest(Guid reference);
    }
}