using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment.Validation.Rejection
{
    public class ExpiryYearValidationTests
    {
        [Theory]
        [MemberData(nameof(InvalidDates))]
        public void Given_An_Invalid_Expiry_When_Validating_Then_Throws_With_Expected_Details(int expiryMonth, int expiryYear)
        {
            var utcNow = new DateTime(2020, 2, 1, 0, 0, 0, DateTimeKind.Utc);
            var payment = Payments.CreatePaymentToBeSaved(expiryMonth: expiryMonth, expiryYear: expiryYear);
            var subject = PaymentTestSubject.WithNoPriorPayments(now: utcNow);
            
            var response = subject.PostPaymentHandler.Handle(payment)!;

            Assert.Equal(PaymentStatus.Rejected, response.Status);
        }
        
        [Fact]
        public void Given_A_Valid_Expiry_On_Boundary_When_Validating_Then_Status_Is_Not_Rejected()
        {
            var utcNow = new DateTime(2020, 2, 1, 0, 0, 0, DateTimeKind.Utc);
            var payment = Payments.CreatePaymentToBeSaved(expiryMonth: 3, expiryYear: 2020);
            var subject = PaymentTestSubject.WithNoPriorPayments(now: utcNow);

            var response = subject.PostPaymentHandler.Handle(payment);
            
            Assert.Equal(PaymentStatus.Authorized, response.Status);
        }
        
        public static IEnumerable<object[]> InvalidDates => 
        [
            [1, 2018],
            [1, 2019],
            [1, 2020],
            [2, 2020],
        ];
    }
}