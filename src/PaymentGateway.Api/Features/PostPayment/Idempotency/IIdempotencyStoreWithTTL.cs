using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;

namespace PaymentGateway.Api.Features.PostPayment.Idempotency
{
    public interface IIdempotencyStoreWithTTL
    {
        Task<bool> Add(CustomerReference reference);
    }
}