using PaymentGateway.Api.Features.PostPayment.Idempotency;

namespace PaymentGateway.Api.Infrastructure.Fakes
{
    public class FakeIdempotencyRepository : IIdempotencyStoreWithTTL
    {
        public List<(string CardNumber, int Amount)> Attempts = new();
        
        public Task<bool> Add(string cardNumber, int amount)
        {
            Attempts.Add((cardNumber, amount));
            return Task.FromResult(Attempts.Count == 1);
        }
    }
}