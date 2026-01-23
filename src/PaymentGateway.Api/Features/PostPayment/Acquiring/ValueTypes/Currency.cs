namespace PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes
{
    public class Currency(string currency)
    {
        public static implicit operator Currency(string currency) => new(currency);
        
        public bool IsValid { get; } = currency.Length is 3;
        
        override public string ToString() => currency;
    }
}