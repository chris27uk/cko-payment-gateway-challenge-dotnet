using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment
{
    public class AuthorisationRequestTests
    {
        [Theory]
        [InlineData(12, 2030, "12/2030")]
        [InlineData(1, 2003, "01/2003")]
        public async Task Given_A_Valid_Authorisation_Request_When_Sending_To_Acquiring_Bank_Then_Expiry_Date_Is_Correct(int expiryMonth, int expiryYear, string expectedExpiryDate)
        {
            var payment = Payments.CreatePaymentToBeSaved(expiryYear: expiryYear, expiryMonth: expiryMonth);
            var subject = PaymentTestSubject.WithNoPriorPayments(now: new DateTime(2002, 1, 1));
            
            await subject.PostPaymentHandler.Handle(payment);

            var request = subject.AcquiringBankGateway.Requests.Single();
            Assert.Equal(expectedExpiryDate, request.ExpiryDate.ToString());
        }
        
        [Fact]
        public async Task Given_A_Valid_Authorisation_Request_When_Sending_To_Acquiring_Bank_Then_Card_Number_Is_Correct()
        {
            var expectedCardNumber = "4885569203193554";
            var payment = Payments.CreatePaymentToBeSaved(cardNumber: expectedCardNumber);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var request = subject.AcquiringBankGateway.Requests.Single();
            Assert.Equal(expectedCardNumber, request.CardNumber.ToString());
        }
        
        [Fact]
        public async Task Given_A_Valid_Authorisation_Request_When_Sending_To_Acquiring_Bank_Then_Amount_Is_Correct()
        {
            var expectedAmount = 1050;
            var payment = Payments.CreatePaymentToBeSaved(amount: expectedAmount);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var request = subject.AcquiringBankGateway.Requests.Single();
            Assert.Equal(expectedAmount, request.Amount.Value);
        }
        
        [Fact]
        public async Task Given_A_Valid_Authorisation_Request_When_Sending_To_Acquiring_Bank_Then_Currency_Is_Correct()
        {
            var expectedCurrency = "GBP";
            var payment = Payments.CreatePaymentToBeSaved(currency: expectedCurrency);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var request = subject.AcquiringBankGateway.Requests.Single();
            Assert.Equal(expectedCurrency, request.Currency.ToString());
        }
        
        [Theory]
        [InlineData("345")]
        [InlineData("3456")]
        public async Task Given_A_Valid_Authorisation_Request_When_Sending_To_Acquiring_Bank_Then_Cvv_Is_Correct(string expectedCvv)
        {
            var payment = Payments.CreatePaymentToBeSaved(cvv: expectedCvv);
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            await subject.PostPaymentHandler.Handle(payment);

            var request = subject.AcquiringBankGateway.Requests.Single();
            Assert.Equal(expectedCvv, request.Cvv.Value);
        }
    }
}