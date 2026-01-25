using Microsoft.ApplicationInsights;
using OpenTelemetry;

using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;

namespace PaymentGateway.Api.Infrastructure.Observability
{
    public class ApplicationInsightsTelemetryProbe(TelemetryClient client) : IObservabilityProbe
    {
        public void PaymentDataRejected(string fieldName, CustomerReference customerReference)
        {
            var properties = new Dictionary<string, string>
            {
                { "FieldName", fieldName }, 
                { "Reference", customerReference?.ToString() ?? "" }
            };
            client.TrackEvent("PaymentRejected", properties);
        }

        public void DuplicatePaymentRequest(CustomerReference reference)
        {
            var properties = new Dictionary<string, string> { { "Reference", reference!.Value.ToString() } };
            client.TrackEvent("DuplicatePaymentRequest", properties);
        }
    }
}