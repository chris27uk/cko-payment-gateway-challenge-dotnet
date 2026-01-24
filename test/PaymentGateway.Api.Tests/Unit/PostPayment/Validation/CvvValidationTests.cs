using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;
using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment.Validation
{
    public class CvvValidationTests
    {
        [Theory]
        [MemberData(nameof(InvalidCvvs))]
        public async Task Given_An_Invalid_Expiry_Month_When_Validating_Then_Does_Not_Send_To_Bank(string cvv)
        {
            var payment = Payments.CreatePaymentToBeSaved(cvv: cvv);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            _ = await Record.ExceptionAsync(() => subject.PostPaymentHandler.Handle(payment));
            
            Assert.Empty(subject.AcquiringBankGateway.Requests);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCvvs))]
        public async Task Given_An_Invalid_Cvv_When_Validating_Then_Raises_Observability_Event(string cvv)
        {
            var payment = Payments.CreatePaymentToBeSaved(cvv: cvv);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var evt = subject.ObservabilityProbe.RejectedEvents.Single();
            Assert.Equal("Cvv", evt.FieldName);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCvvs))]
        public async Task Given_An_Invalid_Cvv_When_Validating_Then_Raises_Observability_Event_Without_PD(string cvv)
        {
            var payment = Payments.CreatePaymentToBeSaved(cvv: cvv);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var evt = subject.ObservabilityProbe.RejectedEvents.Single();
            Assert.NotEqual(new CardNumber(payment.CardNumber).LastFourDigits().ToString(), evt.CustomerIdentifier);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCvvs))]
        public async Task Given_An_Invalid_Expiry_Month_When_Validating_Then_Does_Not_Save_Payment(string cvv)
        {
            var payment = Payments.CreatePaymentToBeSaved(cvv: cvv);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.PaymentRepository.Payments);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCvvs))]
        public async Task Given_An_Invalid_Expiry_Month_When_Validating_Then_Status_Is_Rejected(string cvv)
        {
            var payment = Payments.CreatePaymentToBeSaved(cvv: cvv);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            var response = await subject.PostPaymentHandler.Handle(payment)!;

            Assert.Equal(PaymentStatus.Rejected, response.Status);
        }
        
        public static IEnumerable<object[]> InvalidCvvs => 
        [
            ["0"], ["12345"], ["12"], [""]
        ];
    }
}