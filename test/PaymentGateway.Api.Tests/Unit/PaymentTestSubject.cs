using PaymentGateway.Api.Features.GetPayment;
using PaymentGateway.Api.Features.PostPayment;
using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit
{
    public class PaymentTestSubject
    {
        public readonly static Guid DefaultAuthCode = Guid.Parse("4d709b2b-5976-45f4-80d3-901f6886d875");
        
        private PaymentTestSubject(FakePaymentsRepository repository, FakeAcquiringBankGateway acquiringBankGateway, FakeDateTimeProvider dateTimeProvider)
        {
            this.PaymentRepository = repository;
            this.GetPaymentsHandler = new GetPaymentHandler(repository);
            this.ObservabilityProbe = new FakeObservabilityProbe();
            this.Obfuscation = new FakeObfuscation();
            this.PostPaymentHandler = new PostPaymentHandler(repository, acquiringBankGateway, dateTimeProvider, this.Obfuscation, this.ObservabilityProbe);
            this.AcquiringBankGateway = acquiringBankGateway;
        }
        
        public FakePaymentsRepository PaymentRepository { get; }
        
        public GetPaymentHandler GetPaymentsHandler { get; }
        
        public PostPaymentHandler PostPaymentHandler { get; }
        
        public FakeAcquiringBankGateway AcquiringBankGateway { get; }
        
        public FakeObservabilityProbe ObservabilityProbe { get; }
        
        public FakeObfuscation Obfuscation { get; }
        
        public static PaymentTestSubject WithPayment(PostPaymentResponse savedPayment)
        {
            var now = new DateTime(2021, 1, 1);
            var repository = new FakePaymentsRepository();
            repository.Add(savedPayment);
            return new PaymentTestSubject(repository, new FakeAcquiringBankGateway(true, DefaultAuthCode), new FakeDateTimeProvider(now));
        }

        public static PaymentTestSubject WithNoPriorPayments(Guid? authCode = null, DateTime? now = null, bool shouldAuthorise = true)
        {
            authCode ??= DefaultAuthCode;
            now ??= new DateTime(2021, 1, 1);
            return new(new FakePaymentsRepository(), new FakeAcquiringBankGateway(shouldAuthorise, authCode.Value), new FakeDateTimeProvider(now.Value));
        }
    }
}