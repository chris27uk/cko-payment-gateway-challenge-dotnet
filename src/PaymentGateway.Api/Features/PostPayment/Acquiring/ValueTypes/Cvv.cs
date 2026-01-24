namespace PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes
{
    public class Cvv(string cvv)
    {
        public static implicit operator Cvv(string cvv) => new(cvv);
        
        public bool IsValid { get; } = cvv.ToString().Length is 3 or 4;

        public string Value { get; } = cvv;
    }
}