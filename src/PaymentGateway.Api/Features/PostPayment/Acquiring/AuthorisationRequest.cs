using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;
using PaymentGateway.Api.Infrastructure;

namespace PaymentGateway.Api.Features.PostPayment.Acquiring
{
    public class AuthorisationRequest(CardNumber cardNumber, ExpiryDate expiryDate, Currency currency, AuthorisationAmount amount, Cvv cvv, Guid reference)
    {
        public CardNumber CardNumber { get; } = cardNumber;
        
        public ExpiryDate ExpiryDate { get; } = expiryDate;
        
        public Currency Currency { get; } = currency;
        
        public AuthorisationAmount Amount { get; } = amount;

        public Guid Reference { get; } = reference;
        
        public Cvv Cvv { get; } = cvv;

        public bool Validate(IDateTimeProvider dateTimeProvider, IObservabilityProbe observabilityProbe)
        {
            if (!Cvv.IsValid)
            {
                observabilityProbe.PaymentDataRejected("Cvv", Reference);
                return false;
            }

            if (!Amount.IsValid)
            {
                observabilityProbe.PaymentDataRejected("Amount", Reference);
                return false;
            }

            if (!Currency.IsValid)
            {
                observabilityProbe.PaymentDataRejected("Currency", Reference);
                return false;
            }

            if (!ExpiryDate.IsValid(dateTimeProvider))
            {
                observabilityProbe.PaymentDataRejected("ExpiryDate", Reference);
                return false;
            }
            
            if (!CardNumber.IsValid)
            {
                observabilityProbe.PaymentDataRejected("CardNumber", Reference);
                return false;
            }

            return true;
        }
    }
}