using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure.Persistence;

namespace PaymentGateway.Api.Infrastructure.Fakes;

public class FakePaymentsRepository(bool failsAndRecovers, bool permanentlyFails, PostPaymentResponse[] payments) : IPaymentsRepository
{
    public List<PostPaymentResponse> Payments = [..payments];
    
    public int AttemptCount { get; private set; }
    
    public void Add(PostPaymentResponse payment)
    {
        AttemptCount++;
        if (failsAndRecovers && AttemptCount == 1)
        {
            throw new Exception();
        }
        
        if (permanentlyFails )
        {
            throw new Exception();
        }
        
        Payments.Add(payment);
    }

    public PostPaymentResponse? Get(Guid id)
    {
        AttemptCount++;
        
        if (failsAndRecovers && AttemptCount == 1)
        {
            throw new Exception();
        }
        
        if (permanentlyFails)
        {
            throw new Exception();
        }
        
        return Payments.FirstOrDefault(p => p.Id == id);
    }
}