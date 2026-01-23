using PaymentGateway.Api.Features.PostPayment.Presentation;

namespace PaymentGateway.Api.Infrastructure;

public class FakePaymentsRepository() : IPaymentsRepository
{
    public List<PostPaymentResponse> Payments = new();
    
    public void Add(PostPaymentResponse payment)
    {
        Payments.Add(payment);
    }

    public PostPaymentResponse Get(Guid id)
    {
        return Payments.FirstOrDefault(p => p.Id == id);
    }
}