using Polly;
using Polly.Retry;

namespace PaymentGateway.Api.Features.PostPayment.Acquiring
{
    public class AcquiringBankResiliency(IAcquiringBankGateway acquiringBankGateway) : IAcquiringBankGateway
    {
        private readonly RetryPolicy _policy = Policy.Handle<AcquiringBankTransientError>().Retry();

        public AuthorisationResponse AuthorisePayment(AuthorisationRequest request)
        {
            return _policy.Execute(() => acquiringBankGateway.AuthorisePayment(request));
        }
    }
}