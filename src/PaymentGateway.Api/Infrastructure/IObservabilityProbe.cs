namespace PaymentGateway.Api.Infrastructure
{
    public interface IObservabilityProbe
    {
        void PaymentRequestRejected(string fieldName, string customerIdentifier);
    }
}