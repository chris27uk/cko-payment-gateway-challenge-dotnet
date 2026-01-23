using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment.Validation
{
    public class ExpiryMonthValidationTests
    {
        [Theory]
        [MemberData(nameof(InvalidMonths))]
        public void Given_An_Invalid_Expiry_Month_When_Validating_Then_Does_Not_Send_To_Bank(int expiryMonth)
        {
            var payment = Payments.CreatePaymentToBeSaved(expiryMonth: expiryMonth);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.AcquiringBankGateway.Requests);
        }
        
        [Theory]
        [MemberData(nameof(InvalidMonths))]
        public void Given_An_Invalid_Expiry_Month_When_Validating_Then_Does_Not_Save_Payment(int expiryMonth)
        {
            var payment = Payments.CreatePaymentToBeSaved(expiryMonth: expiryMonth);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.PaymentRepository.Payments);
        }
        
        [Theory]
        [MemberData(nameof(InvalidMonths))]
        public void Given_An_Invalid_Expiry_Month_When_Validating_Then_Throws_With_Expected_Details(int expiryMonth)
        {
            var payment = Payments.CreatePaymentToBeSaved(expiryMonth: expiryMonth);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            var response = subject.PostPaymentHandler.Handle(payment);

            Assert.Equal(PaymentStatus.Rejected, response.Status);
        }
        
        public static IEnumerable<object[]> InvalidMonths => 
        [
            [0], [13], [200]
        ];
    }
}