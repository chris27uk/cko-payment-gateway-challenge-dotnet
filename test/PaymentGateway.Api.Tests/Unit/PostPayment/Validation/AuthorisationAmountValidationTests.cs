using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment.Validation
{
    public class AuthorisationAmountValidationTests
    {
        [Theory]
        [MemberData(nameof(InvalidAmounts))]
        public async Task Given_An_Invalid_Authorisation_Amount_When_Validating_Then_Does_Not_Save_Payment(int amount)
        {
            var payment = Payments.CreatePaymentToBeSaved(amount: amount);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.PaymentRepository.Payments);
        }
        
        [Theory]
        [MemberData(nameof(InvalidAmounts))]
        public async Task Given_An_Invalid_Authorisation_Amount_When_Validating_Then_Does_Not_Send_To_Bank(int amount)
        {
            var payment = Payments.CreatePaymentToBeSaved(amount: amount);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.AcquiringBankGateway.Requests);
        }

        [Theory]
        [MemberData(nameof(InvalidAmounts))]
        public async Task Given_An_Invalid_Authorisation_Amount_When_Validating_Then_Should_Raise_Observability_Event(int amount)
        {
            var payment = Payments.CreatePaymentToBeSaved(amount: amount);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var evt = subject.ObservabilityProbe.RejectedEvents.Single();
            Assert.Equal("Amount", evt.FieldName);
        }
        
        [Theory]
        [MemberData(nameof(InvalidAmounts))]
        public async Task Given_An_Invalid_Authorisation_Amount_When_Validating_Then_Should_Raise_Observability_Event_With_Customer_Reference(int amount)
        {
            var customerReference = Guid.Parse("3de964a7-659f-42d7-b4e9-4a801863b007");
            var payment = Payments.CreatePaymentToBeSaved(amount: amount, reference: customerReference);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var evt = subject.ObservabilityProbe.RejectedEvents.Single();
            Assert.Equal(customerReference, evt.CustomerReference);
        }
        
        [Theory]
        [MemberData(nameof(InvalidAmounts))]
        public async Task Given_An_Invalid_Authorisation_Amount_When_Validating_Then_Status_Is_Rejected(int amount)
        {
            var payment = Payments.CreatePaymentToBeSaved(amount: amount);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            var response = await subject.PostPaymentHandler.Handle(payment)!;

            Assert.Equal(PaymentStatus.Rejected, response.Status);
        }
        
        public static IEnumerable<object[]> InvalidAmounts => 
        [
            [0],
            [-1]
        ];
    }
}