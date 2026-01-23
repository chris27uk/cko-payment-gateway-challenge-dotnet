namespace PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes
{
    public class ExpiryDate(int month, int year)
    {
        public static ExpiryDate From(int expiryMonth, int expiryYear) => new(expiryMonth, expiryYear);

        public override string ToString()
        {
            return $"{month:00}/{year:0000}";
        }
    }
}