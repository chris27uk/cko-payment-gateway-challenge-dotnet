using PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes;
using PaymentGateway.Api.Features.PostPayment.Idempotency;

namespace PaymentGateway.Api.Infrastructure.Fakes
{
    public class FakeIdempotencyRepository : IIdempotencyStoreWithTTL
    {
        public List<CustomerReference> Attempts = new();
        
        public int SaveCount { get; private set; }

        public Task<bool> Add(CustomerReference reference)
        {
            Attempts.Add(reference);

            if (Attempts.Count == 1)
            {
                SaveCount++;
            }
            return Task.FromResult(Attempts.Count == 1);
        }
    }
}