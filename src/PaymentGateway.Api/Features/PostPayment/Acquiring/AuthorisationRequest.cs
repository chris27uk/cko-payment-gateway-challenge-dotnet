using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;
using PaymentGateway.Api.Infrastructure;

namespace PaymentGateway.Api.Features.PostPayment.Acquiring
{
    public class AuthorisationRequest(CardNumber cardNumber, ExpiryDate expiryDate, Currency currency, AuthorisationAmount amount, Cvv cvv)
    {
        public CardNumber CardNumber { get; } = cardNumber;
        
        public ExpiryDate ExpiryDate { get; } = expiryDate;
        
        public Currency Currency { get; } = currency;
        
        public AuthorisationAmount Amount { get; } = amount;
        
        public Cvv Cvv { get; } = cvv;

        public bool Validate(IDateTimeProvider dateTimeProvider)
        {
            if (!cvv.IsValid)
            {
                return false;
            }

            if (!amount.IsValid)
            {
                return false;
            }

            if (!currency.IsValid)
            {
                return false;
            }

            if (!expiryDate.IsValid(dateTimeProvider))
            {
                return false;
            }
            
            if (!cardNumber.IsValid)
            {
                return false;
            }

            return true;
        }
    }
}