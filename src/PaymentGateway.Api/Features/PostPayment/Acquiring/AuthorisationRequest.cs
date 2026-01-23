using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;

namespace PaymentGateway.Api.Features.PostPayment.Acquiring
{
    public class AuthorisationRequest(CardNumber cardNumber, ExpiryDate expiryDate, Currency currency, AuthorisationAmount amount, Cvv cvv)
    {
        public CardNumber CardNumber { get; } = cardNumber;
        
        public ExpiryDate ExpiryDate { get; } = expiryDate;
        
        public Currency Currency { get; } = currency;
        
        public AuthorisationAmount Amount { get; } = amount;
        
        public Cvv Cvv { get; } = cvv;
    }
}