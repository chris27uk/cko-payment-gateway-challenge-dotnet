using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;
using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment.Validation
{
    public class CurrencyValidationTests
    {
        [Theory]
        [MemberData(nameof(InvalidCurrencies))]
        public async Task Given_An_Invalid_Currency_When_Validating_Then_Does_Not_Save_Payment(string currency)
        {
            var payment = Payments.CreatePaymentToBeSaved(currency: currency);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.PaymentRepository.Payments);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCurrencies))]
        public async Task Given_An_Invalid_Currency_When_Validating_Then_Does_Not_Send_To_Bank(string currency)
        {
            var payment = Payments.CreatePaymentToBeSaved(currency: currency);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);
            
            Assert.Empty(subject.AcquiringBankGateway.Requests);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCurrencies))]
        public async Task Given_An_Invalid_Currency_When_Validating_Then_Returns_Status_Rejected(string currency)
        {
            var payment = Payments.CreatePaymentToBeSaved(currency: currency);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            var response = await subject.PostPaymentHandler.Handle(payment)!;

            Assert.Equal(PaymentStatus.Rejected, response.Status);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCurrencies))]
        public async Task Given_An_Invalid_Currency_When_Validating_Then_Raises_Observability_Event(string currency)
        {
            var payment = Payments.CreatePaymentToBeSaved(currency: currency);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var evt = subject.ObservabilityProbe.RejectedEvents.Single();
            Assert.Equal("Currency", evt.FieldName);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCurrencies))]
        public async Task Given_An_Invalid_Currency_When_Validating_Then_Raises_Observability_Event_With_CustomerReference(string currency)
        {
            var customerReference = Guid.Parse("3de964a7-659f-42d7-b4e9-4a801863b007");
            var payment = Payments.CreatePaymentToBeSaved(currency: currency, reference: customerReference);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var evt = subject.ObservabilityProbe.RejectedEvents.Single();
            Assert.Equal(customerReference, evt.CustomerReference);
        }
        
        public static IEnumerable<object[]> InvalidCurrencies => 
        [
            ["XYZA"]
        ];
    }
}