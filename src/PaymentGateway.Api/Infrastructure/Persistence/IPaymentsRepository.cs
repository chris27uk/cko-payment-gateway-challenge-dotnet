using PaymentGateway.Api.Features.PostPayment.Presentation;

namespace PaymentGateway.Api.Infrastructure.Persistence
{
    public interface IPaymentsRepository
    {
        PostPaymentResponseStored? Get(Guid id);

        void Add(PostPaymentResponseStored payment);
    }
}