namespace PaymentGateway.Api.Infrastructure.Persistence
{
    public interface IPaymentsRepository
    {
        PostPaymentResponseStored? Get(Guid id, CancellationToken cancellationToken = default);

        void Add(PostPaymentResponseStored payment, CancellationToken cancellationToken = default);
    }
}