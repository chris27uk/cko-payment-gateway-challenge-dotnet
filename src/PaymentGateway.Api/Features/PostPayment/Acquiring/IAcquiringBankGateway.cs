namespace PaymentGateway.Api.Features.PostPayment.Acquiring
{
    public interface IAcquiringBankGateway
    {
        AuthorisationResponse AuthorisePayment(AuthorisationRequest request);
    }
}