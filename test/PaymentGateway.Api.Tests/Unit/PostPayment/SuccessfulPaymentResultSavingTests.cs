using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment
{
    public class SuccessfulPaymentResultSavingTests
    {
        [Fact]
        public void Given_A_Valid_Payment_When_Saving_Then_Saves_Correct_Expiry_Month()
        {
            var expectedExpiryMonth = 12;
            var payment = Payments.CreatePaymentToBeSaved(expiryMonth: expectedExpiryMonth);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            subject.PostPaymentHandler.Handle(payment);

            var savedPaymentDetails = subject.PaymentRepository.Payments.Single();
            Assert.Equal(expectedExpiryMonth, savedPaymentDetails.ExpiryMonth);
        }
        
        [Fact]
        public void Given_A_Valid_Payment_When_Saving_Then_Saves_Correct_Expiry_Year()
        {
            var expectedExpiryYear = 2020;
            var payment = Payments.CreatePaymentToBeSaved(expiryYear: expectedExpiryYear);
            var subject = PaymentTestSubject.WithNoPriorPayments(now: new DateTime(2002, 1, 1));
            
            subject.PostPaymentHandler.Handle(payment);

            var savedPaymentDetails = subject.PaymentRepository.Payments.Single();
            Assert.Equal(expectedExpiryYear, savedPaymentDetails.ExpiryYear);
        }
        
        [Fact]
        public void Given_A_Valid_Payment_When_Saving_Then_Saves_Correct_Amount()
        {
            var expectedAmount = 100;
            var payment = Payments.CreatePaymentToBeSaved(amount: expectedAmount);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            subject.PostPaymentHandler.Handle(payment);

            var savedPaymentDetails = subject.PaymentRepository.Payments.Single();
            Assert.Equal(expectedAmount, savedPaymentDetails.Amount);
        }
        
        [Fact]
        public void Given_A_Valid_Payment_When_Saving_Then_Saves_Card_Number_Last_Four()
        {
            var expectedCardNumber = "348001494318264";
            var payment = Payments.CreatePaymentToBeSaved(cardNumber: expectedCardNumber);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            subject.PostPaymentHandler.Handle(payment);

            var savedPaymentDetails = subject.PaymentRepository.Payments.Single();
            Assert.Equal(8264, savedPaymentDetails.CardNumberLastFour);
        }
        
        [Theory]
        [InlineData("GBP")]
        [InlineData("USD")]
        public void Given_A_Valid_Payment_When_Saving_Then_Saves_Currency(string expectedCurrency)
        {
            var payment = Payments.CreatePaymentToBeSaved(currency: expectedCurrency);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            subject.PostPaymentHandler.Handle(payment);

            var savedPaymentDetails = subject.PaymentRepository.Payments.Single();
            Assert.Equal(expectedCurrency, savedPaymentDetails.Currency);
        }
        
        [Fact]
        public void Given_A_Valid_Payment_When_Saving_Then_Saves_Authorisation_Code()
        {
            var expectedAuthCode = Guid.Parse("4d709b2b-5976-45f4-80d3-901f6886d875");
            var payment = Payments.CreatePaymentToBeSaved();
            var subject = PaymentTestSubject.WithNoPriorPayments(authCode: expectedAuthCode);
            
            subject.PostPaymentHandler.Handle(payment);

            var savedPaymentDetails = subject.PaymentRepository.Payments.Single();
            Assert.Equal(expectedAuthCode, savedPaymentDetails.Id);
        }
    }
}