using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment.Validation.Rejection
{
    public class AuthorisationAmountValidationTests
    {
        [Theory]
        [MemberData(nameof(InvalidAmounts))]
        public void Given_An_Invalid_Authorisation_Amount_When_Validating_Then_Does_Not_Save_Payment(int amount)
        {
            var payment = Payments.CreatePaymentToBeSaved(amount: amount);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.PaymentRepository.Payments);
        }
        
        [Theory]
        [MemberData(nameof(InvalidAmounts))]
        public void Given_An_Invalid_Authorisation_Amount_When_Validating_Then_Does_Not_Send_To_Bank(int amount)
        {
            var payment = Payments.CreatePaymentToBeSaved(amount: amount);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.AcquiringBankGateway.Requests);
        }
        
        [Theory]
        [MemberData(nameof(InvalidAmounts))]
        public void Given_An_Invalid_Authorisation_Amount_When_Validating_Then_Status_Is_Rejected(int amount)
        {
            var payment = Payments.CreatePaymentToBeSaved(amount: amount);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            var response = subject.PostPaymentHandler.Handle(payment)!;

            Assert.Equal(PaymentStatus.Rejected, response.Status);
        }
        
        public static IEnumerable<object[]> InvalidAmounts => 
        [
            [0],
            [-1]
        ];
    }
}