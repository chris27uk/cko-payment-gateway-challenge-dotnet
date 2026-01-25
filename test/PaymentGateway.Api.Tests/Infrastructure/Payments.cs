using PaymentGateway.Api.Features.PostPayment.Acquiring;
using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;
using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure.Persistence;
using PaymentGateway.Api.Shared;

namespace PaymentGateway.Api.Tests.Infrastructure
{
    public static class Payments
    {
        public static Guid DefaultId = Guid.Parse("c741d457-6d55-4ed2-afdb-f348fcd42e8a");
        public static Guid DefaultAuthCode = Guid.Parse("c741d45e-6d55-4ed2-afdb-f348fcd42e8a");

        public static AuthorisationRequest CreateAuthorisationRequest(
            int expiryYear = 2030,
            int expiryMonth = 11,
            int amount = 100,
            string cardNumber = "348001494318264",
            Guid? reference = null, 
            string cvv = "123",
            string currency = "GBP")
        {
            reference ??= DefaultId;
            return new AuthorisationRequest(cardNumber, ExpiryDate.From(expiryMonth, expiryYear), currency, amount, cvv, reference.Value);
        }
        
        public static PostPaymentRequest CreatePaymentToBeSaved(
            int expiryYear = 2030,
            int expiryMonth = 11,
            int amount = 100,
            Guid? reference = null, 
            string cardNumber = "348001494318264",
            string cvv = "123",
            string currency = "GBP")
        {
            reference ??= DefaultId;
            return new PostPaymentRequest
            {
                ExpiryYear = expiryYear,
                ExpiryMonth = expiryMonth,
                Amount = amount,
                Reference = reference ?? DefaultId,
                Cvv = cvv,
                CardNumber = cardNumber,
                Currency = currency
            };
        }

        public static PostPaymentResponseStored CreateSavedPayment(
            Guid? id = null,
            Guid? authorisationCode = null,
            int expiryYear = 2030,
            int expiryMonth = 11,
            int amount = 100,
            int cardNumberLastFour = 1111,
            string currency = "GBP",
            PaymentStatus paymentStatus = PaymentStatus.Authorized)
        {
            id ??= DefaultId;
            authorisationCode ??= DefaultAuthCode;
            return new PostPaymentResponseStored
            {
                Id = id.Value,
                ExpiryYear = expiryYear,
                ExpiryMonth = expiryMonth,
                Amount = amount,
                CardNumberLastFour = cardNumberLastFour,
                Currency = currency,
                Status = paymentStatus,
                AuthorisationCode = authorisationCode.Value
            };
        }
    }
}