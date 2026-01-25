using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;
using PaymentGateway.Api.Infrastructure;

namespace PaymentGateway.Api.Tests.Infrastructure
{
    // In reality this would call out to a real observability system. I usually use application insights or prometheus
    // given no other guidance and would implement as TelemetryClient.TrackEvent.
    
    public class FakeObservabilityProbe : IObservabilityProbe
    {
        public readonly List<(string FieldName, Guid? CustomerReference)> RejectedEvents = new();
        public readonly List<Guid> DuplicateRequestEvents = new();

        public void DuplicatePaymentRequest(CustomerReference reference)
        {
            DuplicateRequestEvents.Add(reference.Value);
        }
        
        public void PaymentDataRejected(string fieldName, CustomerReference customerReference)
        {
            RejectedEvents.Add((fieldName, customerReference.Value));
        }
    }
}