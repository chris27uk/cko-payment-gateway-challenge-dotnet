using PaymentGateway.Api.Features.GetPayment;
using PaymentGateway.Api.Features.PostPayment;
using PaymentGateway.Api.Features.PostPayment.Contract;
using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit
{
    public class PaymentTestSubject
    {
        public readonly static Guid DefaultAuthCode = Guid.Parse("4d709b2b-5976-45f4-80d3-901f6886d875");
        
        private PaymentTestSubject(FakePaymentsRepository repository, FakeAcquiringBankGateway acquiringBankGateway)
        {
            this.PaymentRepository = repository;
            this.GetPaymentsHandler = new GetPaymentHandler(repository);
            this.PostPaymentHandler = new PostPaymentHandler(repository, acquiringBankGateway);
            this.AcquiringBankGateway = acquiringBankGateway;
        }
        
        public FakePaymentsRepository PaymentRepository { get; }
        
        public GetPaymentHandler GetPaymentsHandler { get; }
        
        public IPostPaymentHandler PostPaymentHandler { get; }
        
        public FakeAcquiringBankGateway AcquiringBankGateway { get; }
        
        public static PaymentTestSubject WithPayment(PostPaymentResponse savedPayment)
        { 
            var repository = new FakePaymentsRepository();
            repository.Add(savedPayment);
            return new PaymentTestSubject(repository, new FakeAcquiringBankGateway(true, DefaultAuthCode));
        }

        public static PaymentTestSubject WithNoPriorPayments(Guid? authCode = null, bool shouldAuthorise = true)
        {
            authCode ??= DefaultAuthCode;
            return new(new FakePaymentsRepository(), new FakeAcquiringBankGateway(shouldAuthorise, authCode.Value));
        }
    }
}