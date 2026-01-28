using PaymentGateway.Api.Infrastructure.Persistence;

namespace PaymentGateway.Api.Infrastructure.Fakes;

public class FakePaymentsRepository(bool failsAndRecovers, bool permanentlyFails, PostPaymentResponseStored[] payments) : IPaymentsRepository
{
    public List<PostPaymentResponseStored> Payments = [..payments];
    public List<CancellationToken> AddCancellationTokens = [];
    public List<CancellationToken> GetCancellationTokens = [];
    
    public int AttemptCount { get; private set; }
    
    public void Add(PostPaymentResponseStored payment, CancellationToken cancellationToken = default)
    {
        AddCancellationTokens.Add(cancellationToken);
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

    public PostPaymentResponseStored? Get(Guid id, CancellationToken cancellationToken = default)
    {
        GetCancellationTokens.Add(cancellationToken);
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