using PaymentGateway.Api.Features.PostPayment.Acquiring;
using PaymentGateway.Api.Infrastructure;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Integration.PostPayment.Acquiring
{
    [Collection("Integration")]

    public class AcquiringTests : IAsyncLifetime
    {
        private HttpClient? _normalClient;
        private HttpClient? _brokenClient;
        
        [Theory]
        [MemberData(nameof(Modes))]
        public async Task Given_A_Request_That_Will_Authorise_When_Authorising_Then_Returns_Authorised(bool useFake)
        {
            var client = CreateGateway(useFake, ScenarioType.WillAuthorise);

            var response = await client.AuthorisePayment(CreateRequest(useFake, ScenarioType.WillAuthorise));
            
            Assert.NotNull(response.AuthorisationCode);
            Assert.True(response.Authorised);
        }
        
        [Theory]
        [MemberData(nameof(Modes))]
        public async Task Given_A_Request_That_Will_Decline_When_Authorising_Then_Returns_Declined(bool useFake)
        {
            var client = CreateGateway(useFake, ScenarioType.WillDecline);

            var response = await client.AuthorisePayment(CreateRequest(useFake, ScenarioType.WillDecline));
            
            Assert.Null(response.AuthorisationCode);
            Assert.False(response.Authorised);
        }
        
        [Theory]
        [MemberData(nameof(Modes))]
        public async Task Given_A_Request_That_Will_Fail_When_Authorising_Then_Throws_Transient_Error(bool useFake)
        {
            var client = CreateGateway(useFake, ScenarioType.WillFail);

            var exception = await Record.ExceptionAsync(async () => await client.AuthorisePayment(CreateRequest(useFake, ScenarioType.WillFail)));
            
            Assert.NotNull(exception);
            Assert.IsType<AcquiringBankTransientError>(exception);
            Assert.IsType<HttpRequestException>(exception.InnerException);
        }
        
        [Theory]
        [MemberData(nameof(Modes))]
        public async Task Given_A_Request_That_Will_Fail_Unexpectedly_When_Authorising_Then_Throws(bool useFake)
        {
            var client = CreateGateway(useFake, ScenarioType.WillFailUnexpectedly);

            var exception = await Record.ExceptionAsync(async () => await client.AuthorisePayment(CreateRequest(useFake, ScenarioType.WillFailUnexpectedly)));
            
            Assert.NotNull(exception);
            Assert.IsType<HttpRequestException>(exception);
        }
        
        public static IEnumerable<object[]> Modes => 
        [
            [true], [false]
        ];

        private static AuthorisationRequest CreateRequest(bool useFake, ScenarioType scenarioType)
        {
            if (useFake)
            {
                return Payments.CreateAuthorisationRequest();
            }
            else
            {
                var cardNumber = scenarioType switch
                {
                    ScenarioType.WillDecline => "348001494318262",
                    ScenarioType.WillFail => "348001494318200",
                    ScenarioType.WillAuthorise => "348001494318261",
                    _ => ""
                };
                return Payments.CreateAuthorisationRequest(cardNumber: cardNumber);
            }
        }
        
        private IAcquiringBankGateway CreateGateway(bool useFake, ScenarioType scenarioType)
        {
            if (useFake)
            {
                return new FakeAcquiringBankGateway(
                    false, 
                    scenarioType == ScenarioType.WillFail, 
                    scenarioType == ScenarioType.WillFailUnexpectedly, 
                    scenarioType == ScenarioType.WillAuthorise, 
                    Guid.NewGuid());
            }
            return new HttpAcquiringBankGateway(scenarioType == ScenarioType.WillFailUnexpectedly ? _brokenClient! : _normalClient!);
        }

        public Task InitializeAsync()
        {
            _normalClient = new HttpClient();
            _normalClient.BaseAddress = new Uri("http://localhost:8080");
            _normalClient.Timeout = TimeSpan.FromSeconds(2);
            _brokenClient = new HttpClient();
            _brokenClient.BaseAddress = new Uri("http://localhost:8081");
            _brokenClient.Timeout = TimeSpan.FromSeconds(2);
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            _brokenClient?.Dispose();
            _normalClient?.Dispose();
            return Task.CompletedTask;
        }
    }

    internal enum ScenarioType
    {
        WillAuthorise,
        WillDecline,
        WillFail,
        WillFailUnexpectedly
    }
}