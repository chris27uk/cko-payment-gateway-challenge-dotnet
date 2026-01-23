namespace PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes
{
    public class AuthorisationAmount(int amount)
    {
        public static implicit operator AuthorisationAmount(int amount) => new(amount);
        
        public int Value { get; } = amount;
    }
}