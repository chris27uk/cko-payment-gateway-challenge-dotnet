using PaymentGateway.Api.Infrastructure;

namespace PaymentGateway.Api.Tests.Infrastructure
{
    // In reality this would call out to a real observability system. I usually use application insights or prometheus
    // given no other guidance and would implement as TelemetryClient.TrackEvent.
    
    public class FakeObservabilityProbe : IObservabilityProbe
    {
        public List<(string FieldName, string CustomerIdentifier)> RejectedEvents = new();
        
        public void PaymentRequestRejected(string fieldName, string CustomerIdentifier)
        {
            RejectedEvents.Add((fieldName, CustomerIdentifier));
        }
    }
}