using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment.Validation
{
    public class CardNumberValidationTests
    {
        [Theory]
        [MemberData(nameof(InvalidCardNumbers))]
        public async Task Given_An_Invalid_CardNumber_When_Validating_Then_Does_Not_Save_Payment(string cardNumber)
        {
            var payment = Payments.CreatePaymentToBeSaved(cardNumber: cardNumber);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.PaymentRepository.Payments);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCardNumbers))]
        public async Task Given_An_Invalid_CardNumber_When_Validating_Then_Does_Not_Send_To_Bank(string cardNumber)
        {
            var payment = Payments.CreatePaymentToBeSaved(cardNumber: cardNumber);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.AcquiringBankGateway.Requests);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCardNumbers))]
        public async Task Given_An_Invalid_CardNumber_When_Validating_Then_Should_Raise_Observability_Event(string cardNumber)
        {
            var payment = Payments.CreatePaymentToBeSaved(cardNumber: cardNumber);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var evt = subject.ObservabilityProbe.RejectedEvents.Single();
            Assert.Equal("CardNumber", evt.FieldName);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCardNumbers))]
        public async Task Given_An_Invalid_CardNumber_When_Validating_Then_Should_Raise_Observability_Event_Without_PD(string cardNumber)
        {
            var payment = Payments.CreatePaymentToBeSaved(cardNumber: cardNumber);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var evt = subject.ObservabilityProbe.RejectedEvents.Single();
            Assert.NotEqual(cardNumber.Length > 0 ? "?" : cardNumber[^4..], evt.CustomerIdentifier);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCardNumbers))]
        public async Task Given_An_Invalid_CardNumber_When_Validating_Then_Status_Is_Rejected(string cardNumber)
        {
            var payment = Payments.CreatePaymentToBeSaved(cardNumber: cardNumber);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            var response = await subject.PostPaymentHandler.Handle(payment);

            Assert.Equal(PaymentStatus.Rejected, response.Status);
        }

        public static IEnumerable<object[]> InvalidCardNumbers => 
        [
            ["3480014943182g4"], ["a"], ["34800149431820400000"], ["3480014943"]
        ];
    }
}