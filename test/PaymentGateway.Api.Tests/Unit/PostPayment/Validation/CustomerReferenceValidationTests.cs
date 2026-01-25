using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment.Validation
{
    public class CustomerReferenceValidationTests
    {
        [Fact]
        public async Task Given_An_Invalid_Customer_Reference_When_Validating_Then_Does_Not_Send_To_Bank()
        {
            var payment = Payments.CreatePaymentToBeSaved(reference: Guid.Empty);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            _ = await Record.ExceptionAsync(() => subject.PostPaymentHandler.Handle(payment));
            
            Assert.Empty(subject.AcquiringBankGateway.Requests);
        }
        
        [Fact]
        public async Task Given_An_Invalid_Customer_Reference_When_Validating_Then_Raises_Observability_Event()
        {
            var payment = Payments.CreatePaymentToBeSaved(reference: Guid.Empty);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var evt = subject.ObservabilityProbe.RejectedEvents.Single();
            Assert.Equal("CustomerReference", evt.FieldName);
        }
        
        [Fact]
        public async Task Given_An_Invalid_Customer_Reference_When_Validating_Then_Raises_Observability_Event_With_CustomerReference()
        {
            var payment = Payments.CreatePaymentToBeSaved(reference: Guid.Empty);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var evt = subject.ObservabilityProbe.RejectedEvents.Single();
            Assert.Equal(Guid.Empty, evt.CustomerReference);
        }
        
        [Fact]
        public async Task Given_An_Invalid_Customer_Reference_When_Validating_Then_Does_Not_Save_Payment()
        {
            var payment = Payments.CreatePaymentToBeSaved(reference: Guid.Empty);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.PaymentRepository.Payments);
        }
        
        [Fact]
        public async Task Given_An_Invalid_Customer_Reference_When_Validating_Then_Status_Is_Rejected()
        {
            var payment = Payments.CreatePaymentToBeSaved(reference: Guid.Empty);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            var response = await subject.PostPaymentHandler.Handle(payment)!;

            Assert.Equal(PaymentStatus.Rejected, response.Status);
        }
    }
}