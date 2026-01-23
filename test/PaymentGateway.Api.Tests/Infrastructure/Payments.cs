using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Shared;

namespace PaymentGateway.Api.Tests.Infrastructure
{
    public static class Payments
    {
        public static Guid DefaultId = Guid.Parse("c741d457-6d55-4ed2-afdb-f348fcd42e8a");

        public static PostPaymentRequest CreatePaymentToBeSaved(
            int expiryYear = 2030,
            int expiryMonth = 11,
            int amount = 100,
            string cardNumber = "348001494318264",
            int cvv = 123,
            string currency = "GBP")
        {
            return new PostPaymentRequest
            {
                ExpiryYear = expiryYear,
                ExpiryMonth = expiryMonth,
                Amount = amount,
                Cvv = cvv,
                CardNumber = cardNumber,
                Currency = currency
            };
        }

        public static PostPaymentResponse CreateSavedPayment(
            Guid? id = null,
            int expiryYear = 2030,
            int expiryMonth = 11,
            int amount = 100,
            int cardNumberLastFour = 1111,
            string currency = "GBP",
            PaymentStatus paymentStatus = PaymentStatus.Authorized)
        {
            id ??= DefaultId;
            return new PostPaymentResponse
            {
                Id = id.Value,
                ExpiryYear = expiryYear,
                ExpiryMonth = expiryMonth,
                Amount = amount,
                CardNumberLastFour = cardNumberLastFour,
                Currency = currency,
                Status = paymentStatus
            };
        }
    }
}