using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment
{
    public class SuccessfulResponseTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(12)]
        public async Task Given_A_Valid_Payment_When_Creating_A_Payment_Then_Saves_Correct_Expiry_Month(int expectedExpiryMonth)
        {
            var payment = Payments.CreatePaymentToBeSaved(expiryMonth: expectedExpiryMonth);
            var subject = PaymentTestSubject.WithNoPriorPayments();

            var response = await subject.PostPaymentHandler.Handle(payment);

            Assert.Equal(expectedExpiryMonth, response.ExpiryMonth);
        }

        [Fact]
        public async Task Given_A_Valid_Payment_When_Creating_A_Payment_Then_Returns_Correct_Expiry_Year()
        {
            var expectedExpiryYear = 2024;
            var payment = Payments.CreatePaymentToBeSaved(expiryYear: expectedExpiryYear);
            var subject = PaymentTestSubject.WithNoPriorPayments();

            var response = await subject.PostPaymentHandler.Handle(payment);

            Assert.Equal(expectedExpiryYear, response.ExpiryYear);
        }

        [Fact]
        public async Task Given_A_Valid_Payment_When_Creating_A_Payment_Then_Returns_Correct_Authorisation_Amount()
        {
            var expectedAmount = 1059;
            var payment = Payments.CreatePaymentToBeSaved(amount: expectedAmount);
            var subject = PaymentTestSubject.WithNoPriorPayments();

            var response = await subject.PostPaymentHandler.Handle(payment);

            Assert.Equal(expectedAmount, response.Amount);
        }

        [Theory]
        [InlineData("4444333322229999919", 9919)]
        [InlineData("444433332222999909", 9909)]
        [InlineData("44443333222299109", 9109)]
        [InlineData("4444333322229309", 9309)]
        [InlineData("444433332222909", 2909)]
        [InlineData("44443333222299", 2299)]
        public async Task Given_A_Valid_Payment_When_Creating_A_Payment_Then_Returns_Correct_Last_Four_Digits(
            string expectedCardNumber, int expectedLastFourDigits)
        {
            var payment = Payments.CreatePaymentToBeSaved(cardNumber: expectedCardNumber);
            var subject = PaymentTestSubject.WithNoPriorPayments();

            var response = await subject.PostPaymentHandler.Handle(payment);

            Assert.Equal(expectedLastFourDigits, response.CardNumberLastFour);
        }

        [Fact]
        public async Task Given_A_Valid_Payment_When_Creating_A_Payment_Then_Returns_Correct_Currency()
        {
            var expectedCurrency = "USD";
            var payment = Payments.CreatePaymentToBeSaved(currency: expectedCurrency);
            var subject = PaymentTestSubject.WithNoPriorPayments();

            var response = await subject.PostPaymentHandler.Handle(payment);

            Assert.Equal(expectedCurrency, response.Currency);
        }

        [Fact]
        public async Task Given_A_Valid_Payment_When_Creating_A_Payment_Then_Returns_Correct_Id()
        {
            var reference = Guid.Parse("5fb08418-5a00-4b70-9f6c-a9523fc7ddbe");
            var payment = Payments.CreatePaymentToBeSaved(reference: reference);
            var subject = PaymentTestSubject.WithNoPriorPayments();

            var response = await subject.PostPaymentHandler.Handle(payment);

            Assert.Equal(reference, response.Id);
        }

        [Fact]
        public async Task
            Given_A_Valid_Payment_That_Will_Authorise_When_Creating_A_Payment_Then_Returns_Payment_Status()
        {
            var payment = Payments.CreatePaymentToBeSaved();
            var subject = PaymentTestSubject.WithNoPriorPayments(shouldAuthorise: true);

            var response = await subject.PostPaymentHandler.Handle(payment);

            Assert.Equal(PaymentStatus.Authorized, response.Status);
        }

        [Fact]
        public async Task Given_A_Valid_Payment_That_Will_Decline_When_Creating_A_Payment_Then_Returns_Payment_Status()
        {
            var payment = Payments.CreatePaymentToBeSaved();
            var subject = PaymentTestSubject.WithNoPriorPayments(shouldAuthorise: false);

            var response = await subject.PostPaymentHandler.Handle(payment);

            Assert.Equal(PaymentStatus.Declined, response.Status);
        }
    }
}