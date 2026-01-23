using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.GetPayment
{
    public class NoPaymentTests
    {
        [Fact]
        public void Given_No_Payment_When_Retrieving_Then_Returns_Nothing()
        {
            var subject = PaymentTestSubject.WithNoPriorPayments();
            
            var response = subject.GetPaymentsHandler.Handle(Payments.DefaultId);
            
            Assert.Null(response);
        }
    }
}