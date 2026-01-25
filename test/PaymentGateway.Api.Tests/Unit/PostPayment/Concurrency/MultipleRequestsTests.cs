using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment.Concurrency
{
    public class MultipleRequestsTests
    {
        [Fact]
        public async Task Given_Multiple_Requests_When_Processing_Payment_Then_Only_One_Is_Saved()
        {
            var payment = Payments.CreatePaymentToBeSaved();
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Single(subject.PaymentRepository.Payments);
        }
        
        [Fact]
        public async Task Given_Multiple_Requests_When_Processing_Payment_Then_Only_One_Is_Processed()
        {
            var payment = Payments.CreatePaymentToBeSaved();
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Single(subject.AcquiringBankGateway.Requests);
        }
        
        [Fact]
        public async Task Given_Multiple_Requests_When_Processing_Payment_Then_First_Is_Authorised()
        {
            var payment = Payments.CreatePaymentToBeSaved();
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            var response1 = await subject.PostPaymentHandler.Handle(payment);
            await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Equal(PaymentStatus.Authorized, response1.Status);
        }
        
        [Fact]
        public async Task Given_Multiple_Requests_When_Processing_Payment_Then_Second_Is_Authorised()
        {
            var payment = Payments.CreatePaymentToBeSaved();
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            var response2 = await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Equal(PaymentStatus.Authorized, response2.Status);
        }
        
        [Fact]
        public async Task Given_Multiple_Requests_When_Processing_Payment_Then_Second_Raises_Event()
        {
            var payment = Payments.CreatePaymentToBeSaved();
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Single(subject.ObservabilityProbe.DuplicateRequestEvents);
        }
        
        [Fact]
        public async Task Given_Multiple_Requests_When_Processing_Payment_Then_Returns_Correct_Amount()
        {
            var expectedAmount = 1059;
            var payment = Payments.CreatePaymentToBeSaved(amount: expectedAmount);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            var response2 = await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Equal(expectedAmount, response2.Amount);
        }
        
        [Fact]
        public async Task Given_Multiple_Requests_When_Processing_Payment_Then_Returns_Last_Four_Digits()
        {
            var cardNumber = "4444333322229";
            var payment = Payments.CreatePaymentToBeSaved(cardNumber: cardNumber);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            var response2 = await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Equal(2229, response2.CardNumberLastFour);
        }
        
        [Fact]
        public async Task Given_Multiple_Requests_When_Processing_Payment_Then_Returns_Expiry_Month()
        {
            var expectedExpiryMonth = 10;
            var payment = Payments.CreatePaymentToBeSaved(expiryMonth: expectedExpiryMonth);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            var response2 = await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Equal(expectedExpiryMonth, response2.ExpiryMonth);
        }
        
        [Fact]
        public async Task Given_Multiple_Requests_When_Processing_Payment_Then_Returns_Expiry_Year()
        {
            var expectedExpiryYear = 2029;
            var payment = Payments.CreatePaymentToBeSaved(expiryYear: expectedExpiryYear);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            var response2 = await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Equal(expectedExpiryYear, response2.ExpiryYear);
        }
        
        [Fact]
        public async Task Given_Multiple_Requests_When_Processing_Payment_Then_Returns_Currency()
        {
            var expectedCurrency = "EUR";
            var payment = Payments.CreatePaymentToBeSaved(currency: expectedCurrency);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            var response2 = await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Equal(expectedCurrency, response2.Currency);
        }
        
        [Fact]
        public async Task Given_Multiple_Requests_When_Processing_Payment_Then_Returns_Id()
        {
            var expectedId = Guid.Parse("5fb08418-5a00-4b70-9f6c-a9523fc7ddbe");
            var payment = Payments.CreatePaymentToBeSaved(reference: expectedId);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            var response2 = await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Equal(expectedId, response2.Id);
        }
    }
}