using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.GetPayment.Concurrency
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
            
            Assert.Equal(1, subject.PaymentRepository.AttemptCount);
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
        public async Task Given_Multiple_Requests_When_Processing_Payment_Then_Second_Is_Rejected()
        {
            var payment = Payments.CreatePaymentToBeSaved();
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            var response2 = await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Equal(PaymentStatus.Rejected, response2.Status);
        }
    }
}