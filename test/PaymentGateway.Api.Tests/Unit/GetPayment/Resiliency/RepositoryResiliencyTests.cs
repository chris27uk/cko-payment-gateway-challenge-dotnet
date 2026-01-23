using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.GetPayment.Resiliency
{
    public class RepositoryResiliencyTests
    {
        [Fact]
        public void Given_A_Valid_Request_When_Repository_Is_Temporarily_Not_Available_Then_Retries()
        {
            var payment = Payments.CreateSavedPayment();
            var subject = PaymentTestSubject.WithPriorPayment(payment, repositoryFailsAndRecovers: true);
            
            _ = Record.Exception(() => subject.GetPaymentsHandler.Handle(payment.Id));
            
            Assert.Equal(2, subject.PaymentRepository.AttemptCount);
        }
        
        [Fact]
        public void Given_A_Valid_Request_When_Repository_Is_Temporarily_Not_Available_Then_Does_Not_Error()
        {
            var payment = Payments.CreateSavedPayment();
            var subject = PaymentTestSubject.WithPriorPayment(payment, repositoryFailsAndRecovers: true);
    
            var ex = Record.Exception(() => subject.GetPaymentsHandler.Handle(payment.Id));

            Assert.Null(ex);
        }
        
        [Fact]
        public void Given_A_Valid_Request_When_Repository_Is_Permanently_Unavailable_Then_Does_Error()
        {
            var payment = Payments.CreateSavedPayment();
            var subject = PaymentTestSubject.WithPriorPayment(payment, repositoryPermanentlyFails: true);
    
            var ex = Record.Exception(() => subject.GetPaymentsHandler.Handle(payment.Id));

            Assert.NotNull(ex);
        }
        
        [Fact]
        public void Given_A_Valid_Request_When_Sending_To_Bank_Experiencing_Unexpected_Errors_Then_Does_Error()
        {
            var payment = Payments.CreateSavedPayment();
            var subject = PaymentTestSubject.WithPriorPayment(payment, repositoryPermanentlyFails: true);
    
            var ex = Record.Exception(() => subject.GetPaymentsHandler.Handle(payment.Id));
            
            Assert.NotNull(ex);
        }
        
        [Fact]
        public void Given_A_Valid_Request_When_Sending_To_Bank_Experiencing_Unexpected_Errors_Then_Does_Not_Retry()
        {
            var payment = Payments.CreateSavedPayment();
            var subject = PaymentTestSubject.WithPriorPayment(payment, repositoryPermanentlyFails: true);
    
            _ = Record.Exception(() => subject.GetPaymentsHandler.Handle(payment.Id));
            
            Assert.Equal(2, subject.PaymentRepository.AttemptCount);
        }
    }
}