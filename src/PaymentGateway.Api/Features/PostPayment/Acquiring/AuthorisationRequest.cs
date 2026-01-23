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
            string customerIdentifier = cardNumber.ToString().Length > 4 ? cardNumber.ToString()[^4..] : string.Empty;
            if (!cvv.IsValid)
            {
                observabilityProbe.PaymentRequestRejected("Cvv", customerIdentifier);
                return false;
            }

            if (!amount.IsValid)
            {
                observabilityProbe.PaymentRequestRejected("Amount", customerIdentifier);
                return false;
            }

            if (!currency.IsValid)
            {
                observabilityProbe.PaymentRequestRejected("Currency", customerIdentifier);
                return false;
            }

            if (!expiryDate.IsValid(dateTimeProvider))
            {
                observabilityProbe.PaymentRequestRejected("ExpiryDate", customerIdentifier);
                return false;
            }
            
            if (!cardNumber.IsValid)
            {
                observabilityProbe.PaymentRequestRejected("CardNumber", customerIdentifier);
                return false;
            }

            return true;
        }
    }
}