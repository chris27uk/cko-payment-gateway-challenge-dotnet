using Polly;
using Polly.Retry;

namespace PaymentGateway.Api.Features.PostPayment.Acquiring
{
    public class ResilientAcquiringBankGateway(IAcquiringBankGateway acquiringBankGateway) : IAcquiringBankGateway
    {
        private readonly AsyncRetryPolicy _policy = Policy.Handle<AcquiringBankTransientError>().RetryAsync();

        public async Task<AuthorisationResponse> AuthorisePayment(AuthorisationRequest request)
        {
            return await _policy.ExecuteAsync(async () => await acquiringBankGateway.AuthorisePayment(request));
        }
    }
}