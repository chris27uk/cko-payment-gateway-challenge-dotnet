using PaymentGateway.Api.Features.GetPayment;
using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit
{
    public class PaymentTestSubject
    {
        public readonly static Guid DefaultAuthCode = Guid.Parse("4d709b2b-5976-45f4-80d3-901f6886d875");
        
        private PaymentTestSubject(FakePaymentsRepository repository)
        {
            this.PaymentRepository = repository;
            this.GetPaymentsHandler = new GetPaymentHandler(repository);
        }
        
        public FakePaymentsRepository PaymentRepository { get; }
        
        public GetPaymentHandler GetPaymentsHandler { get; }
        
        public static PaymentTestSubject WithPayment(PostPaymentResponse savedPayment)
        {
            var now = new DateTime(2021, 1, 1);
            var repository = new FakePaymentsRepository();
            repository.Add(savedPayment);
            return new PaymentTestSubject(repository);
        }

        public static PaymentTestSubject WithNoPriorPayments(Guid? authCode = null, DateTime? now = null, bool shouldAuthorise = true)
        {
            authCode ??= DefaultAuthCode;
            now ??= new DateTime(2021, 1, 1);
            return new(new FakePaymentsRepository());
        }
    }
}