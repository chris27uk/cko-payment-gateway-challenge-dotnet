using Microsoft.ApplicationInsights;
using OpenTelemetry;

namespace PaymentGateway.Api.Infrastructure.Observability
{
    public class ApplicationInsightsTelemetryProbe(TelemetryClient client) : IObservabilityProbe
    {
        public void PaymentDataRejected(string fieldName, Guid? customerReference)
        {
            var properties = new Dictionary<string, string>
            {
                { "FieldName", fieldName }, 
                { "Reference", customerReference?.ToString() ?? "" }
            };
            client.TrackEvent("PaymentRejected", properties);
        }

        public void DuplicatePaymentRequest(Guid reference)
        {
            var properties = new Dictionary<string, string> { { "Reference", reference.ToString() } };
            client.TrackEvent("DuplicatePaymentRequest", properties);
        }
    }
}