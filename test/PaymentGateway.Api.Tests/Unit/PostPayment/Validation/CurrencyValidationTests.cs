using System.Runtime.Loader;

using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;
using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment.Validation
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
        public void Given_An_Invalid_Currency_When_Validating_Then_Returns_Status_Rejected(string currency)
        {
            var payment = Payments.CreatePaymentToBeSaved(currency: currency);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            var response = subject.PostPaymentHandler.Handle(payment)!;

            Assert.Equal(PaymentStatus.Rejected, response.Status);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCurrencies))]
        public void Given_An_Invalid_Currency_When_Validating_Then_Raises_Observability_Event(string currency)
        {
            var payment = Payments.CreatePaymentToBeSaved(currency: currency);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            subject.PostPaymentHandler.Handle(payment);

            var evt = subject.ObservabilityProbe.RejectedEvents.Single();
            Assert.Equal("Currency", evt.FieldName);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCurrencies))]
        public void Given_An_Invalid_Currency_When_Validating_Then_Raises_Observability_Event_Without_PD(string currency)
        {
            var payment = Payments.CreatePaymentToBeSaved(currency: currency);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            subject.PostPaymentHandler.Handle(payment);

            var evt = subject.ObservabilityProbe.RejectedEvents.Single();
            Assert.NotEqual(new CardNumber(payment.CardNumber).LastFourDigits().ToString(), evt.CustomerIdentifier);
        }
        
        public static IEnumerable<object[]> InvalidCurrencies => 
        [
            ["XYZA"]
        ];
    }
}