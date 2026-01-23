namespace PaymentGateway.Api.Infrastructure
{
    public class ObscureLogs(IObservabilityProbe observabilityProbe, IObscureData obscureData) : IObservabilityProbe
    {
        public void PaymentRequestRejected(string fieldName, string customerIdentifier)
        {
            observabilityProbe.PaymentRequestRejected(fieldName, obscureData.Obscure(customerIdentifier));
        }
    }
}