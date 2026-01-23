using PaymentGateway.Api.Features.PostPayment.Presentation;

namespace PaymentGateway.Api.Infrastructure.Persistence
{
    public interface IPaymentsRepository
    {
        PostPaymentResponse? Get(Guid id);

        void Add(PostPaymentResponse payment);
    }
}