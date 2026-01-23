using PaymentGateway.Api.Infrastructure;

namespace PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes
{
    public class ExpiryDate(int month, int year)
    {
        public static ExpiryDate From(int expiryMonth, int expiryYear) => new(expiryMonth, expiryYear);

        public bool IsValid(IDateTimeProvider dateTimeProvider)
        {
            return month is > 0 and <= 12 && new DateTime(year, month, 01) > dateTimeProvider.UtcNow();
        }

        public override string ToString()
        {
            return $"{month:00}/{year:0000}";
        }
    }
}