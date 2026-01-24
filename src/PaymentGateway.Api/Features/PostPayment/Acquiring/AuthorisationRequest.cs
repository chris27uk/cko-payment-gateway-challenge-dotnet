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

        public bool Validate(IDateTimeProvider dateTimeProvider, IObservabilityProbe observabilityProbe)
        {
            string customerIdentifier = CardNumber.ToString().Length > 4 ? CardNumber.ToString()[^4..] : string.Empty;
            if (!Cvv.IsValid)
            {
                observabilityProbe.PaymentRequestRejected("Cvv");
                return false;
            }

            if (!Amount.IsValid)
            {
                observabilityProbe.PaymentRequestRejected("Amount");
                return false;
            }

            if (!Currency.IsValid)
            {
                observabilityProbe.PaymentRequestRejected("Currency");
                return false;
            }

            if (!ExpiryDate.IsValid(dateTimeProvider))
            {
                observabilityProbe.PaymentRequestRejected("ExpiryDate");
                return false;
            }
            
            if (!CardNumber.IsValid)
            {
                observabilityProbe.PaymentRequestRejected("CardNumber");
                return false;
            }

            return true;
        }
    }
}