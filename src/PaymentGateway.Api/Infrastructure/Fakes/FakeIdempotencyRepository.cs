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
            bool success = false;
            if (Attempts.All(r => reference.Value != r.Value))
            {
                SaveCount++;
                success = true;
            }
            
            Attempts.Add(reference);
            return Task.FromResult(success);
        }
    }
}