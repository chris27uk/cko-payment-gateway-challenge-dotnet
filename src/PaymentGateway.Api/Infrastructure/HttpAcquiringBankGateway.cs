using PaymentGateway.Api.Features.PostPayment.Acquiring;

namespace PaymentGateway.Api.Infrastructure
{
    public class HttpAcquiringBankGateway : IAcquiringBankGateway
    {
        public AuthorisationResponse AuthorisePayment(AuthorisationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}