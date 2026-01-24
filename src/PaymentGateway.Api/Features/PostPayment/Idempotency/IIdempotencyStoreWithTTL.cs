namespace PaymentGateway.Api.Features.PostPayment.Idempotency
{
    public interface IIdempotencyStoreWithTTL
    {
        Task<bool> Add(string cardNumber, int amount);
    }
}