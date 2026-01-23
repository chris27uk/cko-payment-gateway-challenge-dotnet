namespace PaymentGateway.Api.Infrastructure
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow();
    }
}