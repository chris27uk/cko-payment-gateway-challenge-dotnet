using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit.PostPayment.Resiliency
{
    public class RepositoryResiliencyTests
    {
        [Fact]
        public async Task Given_A_Valid_Request_When_Repository_Is_Temporarily_Not_Available_Then_Retries()
        {
            var subject = PaymentTestSubject.WithNoPriorPayments(repositoryFailsAndRecovers: true);
            var payment = Payments.CreatePaymentToBeSaved();
    
            _ = await Record.ExceptionAsync(() => subject.PostPaymentHandler.Handle(payment));
            
            Assert.Equal(2, subject.PaymentRepository.AttemptCount);
        }
        
        [Fact]
        public async Task Given_A_Valid_Request_When_Repository_Is_Temporarily_Not_Available_Then_Does_Not_Error()
        {
            var subject = PaymentTestSubject.WithNoPriorPayments(repositoryFailsAndRecovers: true);
            var payment = Payments.CreatePaymentToBeSaved();
    
            var ex = await Record.ExceptionAsync(() => subject.PostPaymentHandler.Handle(payment));

            Assert.Null(ex);
        }
        
        [Fact]
        public async Task Given_A_Valid_Request_When_Repository_Is_Permanently_Unavailable_Then_Does_Error()
        {
            var subject = PaymentTestSubject.WithNoPriorPayments(repositoryPermanentlyFails: true);
            var payment = Payments.CreatePaymentToBeSaved();
    
            var ex = await Record.ExceptionAsync(() => subject.PostPaymentHandler.Handle(payment));

            Assert.NotNull(ex);
        }
        
        [Fact]
        public async Task Given_A_Valid_Request_When_Sending_To_Bank_Experiencing_Unexpected_Errors_Then_Does_Error()
        {
            var subject = PaymentTestSubject.WithNoPriorPayments(repositoryPermanentlyFails: true);
            var payment = Payments.CreatePaymentToBeSaved();
    
            var ex = await Record.ExceptionAsync(() => subject.PostPaymentHandler.Handle(payment));
            
            Assert.NotNull(ex);
        }
        
        [Fact]
        public async Task Given_A_Valid_Request_When_Sending_To_Bank_Experiencing_Unexpected_Errors_Then_Does_Not_Retry()
        {
            var subject = PaymentTestSubject.WithNoPriorPayments(repositoryPermanentlyFails: true);
            var payment = Payments.CreatePaymentToBeSaved();
    
            _ = await Record.ExceptionAsync(() => subject.PostPaymentHandler.Handle(payment));
            
            Assert.Single(subject.AcquiringBankGateway.Requests);
        }
    }
}