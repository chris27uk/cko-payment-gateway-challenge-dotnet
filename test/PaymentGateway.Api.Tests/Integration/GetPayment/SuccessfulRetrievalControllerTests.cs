using System.Net;
using Newtonsoft.Json;
using PaymentGateway.Api.Features.GetPayment.Presentation;
using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Integration.GetPayment
{
    [Collection("Integration")]
    public class GetPaymentControllerTests : IAsyncLifetime
    {
        private HttpResponseMessage? _response;
        private HttpTestSubject? _subject;

        [Fact]
        public void Given_Payment_When_Requesting_A_Payment_Then_Returns_200()
        {
            Assert.Equal(HttpStatusCode.OK, _response?.StatusCode);
        }
        
        [Fact]
        public async Task Given_Payment_When_Requesting_A_Payment_Then_Returns_Body()
        {
            var deserialized = JsonConvert.DeserializeObject<GetPaymentResponse>(await _response!.Content.ReadAsStringAsync());
            Assert.Equal(PaymentStatus.Authorized, deserialized.Status);
        }
        
        public async Task InitializeAsync()
        {
            var payment = Payments.CreateSavedPayment();
            this._subject = HttpTestSubject.WithExistingPayment(payment);
            this._response = await this._subject.HttpClient.GetAsync($"/api/Payments/{payment.Id}");
        }

        public Task DisposeAsync()
        {
            this._subject?.Dispose();
            this._response?.Dispose();
            return Task.CompletedTask;
        }
    }
}