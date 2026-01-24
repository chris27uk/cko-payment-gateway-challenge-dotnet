using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment
{
    public class SuccessfulResponseTests
    {
        [Fact]
        public async Task Given_A_Valid_Payment_When_Creating_A_Payment_Then_Saves_Correct_Expiry_Month()
        {
            var expectedExpiryMonth = 12;
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
        
        [Fact]
        public async Task Given_A_Valid_Payment_When_Creating_A_Payment_Then_Returns_Correct_Last_Four_Digits()
        {
            var expectedCardNumber = "3564564016834062";
            var payment = Payments.CreatePaymentToBeSaved(cardNumber: expectedCardNumber);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            var response = await subject.PostPaymentHandler.Handle(payment);

            Assert.Equal(4062, response.CardNumberLastFour);
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
        public async Task Given_A_Valid_Payment_When_Creating_A_Payment_Then_Returns_Correct_Authorisation_Code()
        {
            var expectedAuthCode = Guid.Parse("5fb08418-5a00-4b70-9f6c-a9523fc7ddbe");
            var payment = Payments.CreatePaymentToBeSaved();
            var subject = PaymentTestSubject.WithNoPriorPayments(authCode: expectedAuthCode);
            
            var response = await subject.PostPaymentHandler.Handle(payment);

            Assert.Equal(expectedAuthCode, response.Id);
        }
        
        [Fact]
        public async Task Given_A_Valid_Payment_That_Will_Authorise_When_Creating_A_Payment_Then_Returns_Payment_Status()
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