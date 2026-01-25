using PaymentGateway.Api.Features.PostPayment.Presentation;

using Polly;

namespace PaymentGateway.Api.Infrastructure.Persistence
{
    public class ResilientPaymentsRepository(IPaymentsRepository paymentsRepository) : IPaymentsRepository
    {
        private readonly Policy _policy = Policy.Handle<Exception>().Retry();

        public PostPaymentResponseStored? Get(Guid id) => _policy.Execute(() => paymentsRepository.Get(id));

        public void Add(PostPaymentResponseStored payment) => _policy.Execute(() => paymentsRepository.Add(payment));
    }
}