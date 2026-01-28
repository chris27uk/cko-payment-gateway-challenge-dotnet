using PaymentGateway.Api.Features.PostPayment.Presentation;

using Polly;

namespace PaymentGateway.Api.Infrastructure.Persistence
{
    public class ResilientPaymentsRepository(IPaymentsRepository paymentsRepository) : IPaymentsRepository
    {
        private readonly Policy _policy = Policy.Handle<Exception>().Retry();

        public PostPaymentResponseStored? Get(Guid id, CancellationToken cancellationToken = default) => _policy.Execute(() => paymentsRepository.Get(id, cancellationToken));

        public void Add(PostPaymentResponseStored payment, CancellationToken cancellationToken = default) => _policy.Execute(() => paymentsRepository.Add(payment, cancellationToken));
    }
}