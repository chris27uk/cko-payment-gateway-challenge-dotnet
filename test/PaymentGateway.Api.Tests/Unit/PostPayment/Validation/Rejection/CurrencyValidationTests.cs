using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment.Validation.Rejection
{
    public class CurrencyValidationTests
    {
        [Theory]
        [MemberData(nameof(InvalidCurrencies))]
        public void Given_An_Invalid_Currency_When_Validating_Then_Does_Not_Save_Payment(string currency)
        {
            var payment = Payments.CreatePaymentToBeSaved(currency: currency);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.PaymentRepository.Payments);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCurrencies))]
        public void Given_An_Invalid_Currency_When_Validating_Then_Does_Not_Send_To_Bank(string currency)
        {
            var payment = Payments.CreatePaymentToBeSaved(currency: currency);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.AcquiringBankGateway.Requests);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCurrencies))]
        public void Given_An_Invalid_Currency_When_Validating_Then_Throws_With_Expected_Details(string currency)
        {
            var payment = Payments.CreatePaymentToBeSaved(currency: currency);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            var response = subject.PostPaymentHandler.Handle(payment)!;

            Assert.Equal(PaymentStatus.Rejected, response.Status);
        }
        
        public static IEnumerable<object[]> InvalidCurrencies => 
        [
            ["XYZA"]
        ];
    }
}