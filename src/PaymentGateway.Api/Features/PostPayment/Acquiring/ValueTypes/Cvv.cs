namespace PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes
{
    public class Cvv(int cvv)
    {
        public static implicit operator Cvv(int cvv) => new(cvv);

        public int Value { get; } = cvv;
    }
}