using PaymentGateway.Api.Infrastructure;

namespace PaymentGateway.Api.Tests.Infrastructure
{
    public class FakeDateTimeProvider(DateTime now) : IDateTimeProvider
    {
        public FakeDateTimeProvider() : this(new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)) { }
        
        public DateTime UtcNow() => now;
    }
}