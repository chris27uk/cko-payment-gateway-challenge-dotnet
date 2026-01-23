namespace PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes
{
    public class Currency(string currency)
    {
        public static implicit operator Currency(string currency) => new(currency);
        
        override public string ToString() => currency;
    }
}