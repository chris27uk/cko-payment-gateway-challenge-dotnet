using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.GetPayment
{
    public class ExistingPaymentTests
    {
        [Fact]
        public void Given_An_Existing_Payment_When_Retrieving_Then_Returns_Id()
        {
            var id = Guid.Parse("c741d457-6d55-4ed2-afdb-f348fcd42e8a");
            var subject = PaymentTestSubject.WithPriorPayment(Payments.CreateSavedPayment(id: id));

            var response = subject.GetPaymentsHandler.Handle(id);
            
            Assert.Equal(id, response?.Id);
        } 
        
        [Fact]
        public void Given_An_Existing_Payment_When_Retrieving_Then_Returns_Expiry_Year()
        {
            var expectedExpiryYear = 2019;
            var subject = PaymentTestSubject.WithPriorPayment(Payments.CreateSavedPayment(expiryYear: expectedExpiryYear));

            var response = subject.GetPaymentsHandler.Handle(Payments.DefaultId);
            
            Assert.Equal(expectedExpiryYear, response?.ExpiryYear);
        } 
        
        [Fact]
        public void Given_An_Existing_Payment_When_Retrieving_Then_Returns_Expiry_Month()
        {
            var expectedExpiryMonth = 1;
            var subject = PaymentTestSubject.WithPriorPayment(Payments.CreateSavedPayment(expiryMonth: expectedExpiryMonth));

            var response = subject.GetPaymentsHandler.Handle(Payments.DefaultId);
            
            Assert.Equal(expectedExpiryMonth, response?.ExpiryMonth);
        } 
        
        [Fact]
        public void Given_An_Existing_Payment_When_Retrieving_Then_Returns_Amount()
        {
            var expectedAmount = 92;
            var subject = PaymentTestSubject.WithPriorPayment(Payments.CreateSavedPayment(amount: expectedAmount));

            var response = subject.GetPaymentsHandler.Handle(Payments.DefaultId);
            
            Assert.Equal(expectedAmount, response?.Amount);
        } 
        
        [Fact]
        public void Given_An_Existing_Payment_When_Retrieving_Then_Returns_Card_Number_Last_Four()
        {
            var expectedCardNumberLastFour = 6687;
            var subject = PaymentTestSubject.WithPriorPayment(Payments.CreateSavedPayment(cardNumberLastFour: expectedCardNumberLastFour));

            var response = subject.GetPaymentsHandler.Handle(Payments.DefaultId);
            
            Assert.Equal(expectedCardNumberLastFour, response?.CardNumberLastFour);
        }
        
        [Theory]
        [InlineData("GBP")]
        [InlineData("USD")]
        public void Given_An_Existing_Payment_When_Retrieving_Then_Returns_Currency(string expectedCurrency)
        {
            var subject = PaymentTestSubject.WithPriorPayment(Payments.CreateSavedPayment(currency: expectedCurrency));

            var response = subject.GetPaymentsHandler.Handle(Payments.DefaultId);
            
            Assert.Equal(expectedCurrency, response?.Currency);
        }
        
        [Theory]
        [InlineData(PaymentStatus.Authorized)]
        [InlineData(PaymentStatus.Declined)]
        [InlineData(PaymentStatus.Rejected)]
        public void Given_An_Existing_Payment_When_Retrieving_Then_Returns_Payment_Status(PaymentStatus expectedStatus)
        {
            var subject = PaymentTestSubject.WithPriorPayment(Payments.CreateSavedPayment(paymentStatus: expectedStatus));

            var response = subject.GetPaymentsHandler.Handle(Payments.DefaultId);
            
            Assert.Equal(expectedStatus, response?.Status);
        }
    }
}