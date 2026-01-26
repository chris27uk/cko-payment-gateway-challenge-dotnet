namespace PaymentGateway.Api.Features.PostPayment.Acquiring
{
    public interface IAcquiringBankGateway
    {
        Task<AuthorisationResponse> AuthorisePayment(AuthorisationRequest request, CancellationToken cancellationToken = default);
    }
}