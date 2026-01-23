using PaymentGateway.Api.Features.PostPayment.Acquiring;

namespace PaymentGateway.Api.Tests.Infrastructure
{
    public class FakeAcquiringBankGateway(
        bool acquiringBankFailsOnFirstAttempt,
        bool acquiringBankAlwaysFailsTransiently,
        bool acquiringBankAlwaysFailsInUnexpectedWay,
        bool shouldAuthorise, 
        Guid authCode) : IAcquiringBankGateway
    {
        private int _attempts = 0;
        
        public List<AuthorisationRequest> Requests { get; } = new();

        public AuthorisationResponse AuthorisePayment(AuthorisationRequest request)
        {
            Requests.Add(request);

            if (acquiringBankAlwaysFailsTransiently)
            {
                throw new AcquiringBankTransientError();
            }
            
            if (acquiringBankAlwaysFailsInUnexpectedWay)
            {
                throw new Exception();
            }
            
            if (acquiringBankFailsOnFirstAttempt && _attempts == 0)
            {
                _attempts++;
                throw new AcquiringBankTransientError();
            }

            return !shouldAuthorise ? AuthorisationResponse.ForRejected() : AuthorisationResponse.ForAuthorised(authCode);
        }
    }
}