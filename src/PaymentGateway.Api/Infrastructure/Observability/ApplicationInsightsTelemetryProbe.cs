using Microsoft.ApplicationInsights;
using OpenTelemetry;

namespace PaymentGateway.Api.Infrastructure.Observability
{
    public class ApplicationInsightsTelemetryProbe(TelemetryClient client) : IObservabilityProbe
    {
        public void PaymentRequestRejected(string fieldName)
        {
            string? customerId = Baggage.GetBaggage("customer.id");
            client.TrackEvent("PaymentRejected", new Dictionary<string, string>
            {
                {"FieldName", fieldName},
                {"CustomerId", customerId ?? ""}
            });
        }
    }
}