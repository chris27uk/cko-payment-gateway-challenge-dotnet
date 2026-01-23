using PaymentGateway.Api.Features.PostPayment.Acquiring;

namespace PaymentGateway.Api.Tests.Infrastructure
{
    public class FakeAcquiringBankGateway(bool succeeds, Guid authCode) : IAcquiringBankGateway
    {
        public List<AuthorisationRequest> Requests { get; } = new();

        public AuthorisationResponse AuthorisePayment(AuthorisationRequest request)
        {
            Requests.Add(request);

            return !succeeds ? AuthorisationResponse.ForRejected() : AuthorisationResponse.ForAuthorised(authCode);
        }
    }
}