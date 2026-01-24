using PaymentGateway.Api.Features.GetPayment;
using PaymentGateway.Api.Features.PostPayment;
using PaymentGateway.Api.Features.PostPayment.Acquiring;
using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure;
using PaymentGateway.Api.Infrastructure.Fakes;
using PaymentGateway.Api.Infrastructure.Persistence;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Unit
{
    public class PaymentTestSubject
    {
        public readonly static Guid DefaultAuthCode = Guid.Parse("4d709b2b-5976-45f4-80d3-901f6886d875");
        
        private PaymentTestSubject(FakePaymentsRepository repository, FakeAcquiringBankGateway acquiringBankGateway, FakeDateTimeProvider dateTimeProvider)
        {
            this.PaymentRepository = repository;
            this.GetPaymentsHandler = new GetPaymentHandler(new ResilientPaymentsRepository(repository));
            this.ObservabilityProbe = new FakeObservabilityProbe();
            this.Obfuscation = new FakeObfuscation();
            var resilientAcquiringBankGateway = new ResilientAcquiringBankGateway(acquiringBankGateway);
            var secureObservabilityProbe = new ObscureLogs(this.ObservabilityProbe, this.Obfuscation);
            this.PostPaymentHandler = new PostPaymentHandler(new ResilientPaymentsRepository(repository), resilientAcquiringBankGateway, dateTimeProvider, secureObservabilityProbe);
            this.AcquiringBankGateway = acquiringBankGateway;
        }
        
        public FakePaymentsRepository PaymentRepository { get; }
        
        public GetPaymentHandler GetPaymentsHandler { get; }
        
        public PostPaymentHandler PostPaymentHandler { get; }
        
        public FakeAcquiringBankGateway AcquiringBankGateway { get; }
        
        public FakeObservabilityProbe ObservabilityProbe { get; }
        
        public FakeObfuscation Obfuscation { get; }
        
        public static PaymentTestSubject WithPriorPayment(PostPaymentResponse savedPayment,
            bool repositoryPermanentlyFails = false,
            bool repositoryFailsAndRecovers = false)
        {
            var now = new DateTime(2021, 1, 1);
            var repository = new FakePaymentsRepository(repositoryFailsAndRecovers, repositoryPermanentlyFails, [savedPayment]);
            return new PaymentTestSubject(repository, new FakeAcquiringBankGateway(false, false, false, true, DefaultAuthCode), new FakeDateTimeProvider(now));
        }

        public static PaymentTestSubject WithNoPriorPayments(
            Guid? authCode = null, 
            DateTime? now = null, 
            bool shouldAuthorise = true,
            bool repositoryPermanentlyFails = false,
            bool repositoryFailsAndRecovers = false,
            bool acquiringBankFailsOnFirstAttempt = false,
            bool acquiringBankAlwaysTransientlyFails = false,
            bool acquiringBankAlwaysFailsInUnexpectedWay = false)
        {
            authCode ??= DefaultAuthCode;
            now ??= new DateTime(2021, 1, 1);
            var bankGateway = new FakeAcquiringBankGateway(acquiringBankFailsOnFirstAttempt, acquiringBankAlwaysTransientlyFails, acquiringBankAlwaysFailsInUnexpectedWay, shouldAuthorise, authCode.Value);
            var paymentsRepository = new FakePaymentsRepository(repositoryFailsAndRecovers, repositoryPermanentlyFails, []);
            return new(paymentsRepository, bankGateway, new FakeDateTimeProvider(now.Value));
        }
    }
}