namespace PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes
{
    public class Cvv(int cvv)
    {
        public static implicit operator Cvv(int cvv) => new(cvv);
        
        public bool IsValid { get; } = cvv.ToString().Length is 3 or 4;

        public int Value { get; } = cvv;
    }
}