using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;
using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment.Validation
{
    public class ExpiryMonthValidationTests
    {
        [Theory]
        [MemberData(nameof(InvalidMonths))]
        public async Task Given_An_Invalid_Expiry_Month_When_Validating_Then_Does_Not_Send_To_Bank(int expiryMonth)
        {
            var payment = Payments.CreatePaymentToBeSaved(expiryMonth: expiryMonth);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.AcquiringBankGateway.Requests);
        }
        
        [Theory]
        [MemberData(nameof(InvalidMonths))]
        public async Task Given_An_Invalid_Expiry_Month_When_Validating_Then_Does_Not_Save_Payment(int expiryMonth)
        {
            var payment = Payments.CreatePaymentToBeSaved(expiryMonth: expiryMonth);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.PaymentRepository.Payments);
        }
        
        [Theory]
        [MemberData(nameof(InvalidMonths))]
        public async Task Given_An_Invalid_Expiry_Month_When_Validating_Then_Raises_Observability_Event(int expiryMonth)
        {
            var payment = Payments.CreatePaymentToBeSaved(expiryMonth: expiryMonth);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var evt = subject.ObservabilityProbe.RejectedEvents.Single();
            Assert.Equal("ExpiryDate", evt.FieldName);
        }
        
        [Theory]
        [MemberData(nameof(InvalidMonths))]
        public async Task Given_An_Invalid_Expiry_Month_When_Validating_Then_Raises_Observability_Event_Without_PD(int expiryMonth)
        {
            var payment = Payments.CreatePaymentToBeSaved(expiryMonth: expiryMonth);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var evt = subject.ObservabilityProbe.RejectedEvents.Single();
            Assert.NotEqual(new CardNumber(payment.CardNumber).LastFourDigits().ToString(), evt.CustomerIdentifier);
        }
        
        [Theory]
        [MemberData(nameof(InvalidMonths))]
        public async Task Given_An_Invalid_Expiry_Month_When_Validating_Then_Throws_With_Expected_Details(int expiryMonth)
        {
            var payment = Payments.CreatePaymentToBeSaved(expiryMonth: expiryMonth);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            var response = await subject.PostPaymentHandler.Handle(payment);

            Assert.Equal(PaymentStatus.Rejected, response.Status);
        }
        
        public static IEnumerable<object[]> InvalidMonths => 
        [
            [0], [13], [200]
        ];
    }
}