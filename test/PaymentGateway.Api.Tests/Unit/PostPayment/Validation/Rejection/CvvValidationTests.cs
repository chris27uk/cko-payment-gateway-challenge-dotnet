using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment.Validation.Rejection
{
    public class CvvValidationTests
    {
        [Theory]
        [MemberData(nameof(InvalidCvvs))]
        public void Given_An_Invalid_Expiry_Month_When_Validating_Then_Does_Not_Send_To_Bank(int cvv)
        {
            var payment = Payments.CreatePaymentToBeSaved(cvv: cvv);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            _ = Record.Exception(() => subject.PostPaymentHandler.Handle(payment));
            
            Assert.Empty(subject.AcquiringBankGateway.Requests);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCvvs))]
        public void Given_An_Invalid_Expiry_Month_When_Validating_Then_Does_Not_Save_Payment(int cvv)
        {
            var payment = Payments.CreatePaymentToBeSaved(cvv: cvv);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.PaymentRepository.Payments);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCvvs))]
        public void Given_An_Invalid_Expiry_Month_When_Validating_Then_Status_Is_Rejected(int cvv)
        {
            var payment = Payments.CreatePaymentToBeSaved(cvv: cvv);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            var response = subject.PostPaymentHandler.Handle(payment)!;

            Assert.Equal(PaymentStatus.Rejected, response.Status);
        }
        
        public static IEnumerable<object[]> InvalidCvvs => 
        [
            [0], [12345], [12]
        ];
    }
}