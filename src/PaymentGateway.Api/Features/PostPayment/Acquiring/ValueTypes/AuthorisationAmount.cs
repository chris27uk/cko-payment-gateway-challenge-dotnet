namespace PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes
{
    public class AuthorisationAmount(int amount)
    {
        public static implicit operator AuthorisationAmount(int amount) => new(amount);

        public bool IsValid { get; } = amount > 0;
        
        public int Value { get; } = amount;
    }
}