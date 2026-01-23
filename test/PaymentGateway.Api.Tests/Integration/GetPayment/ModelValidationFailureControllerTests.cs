using System.Net;

using Newtonsoft.Json;

using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Shared;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Integration.GetPayment
{
    public class ModelValidationFailureControllerTests : IAsyncLifetime
    {
        private HttpResponseMessage? _response;

        [Fact]
        public void Given_A_Badly_Formatted_Request_When_Retrieving_A_Payment_Then_Returns_400()
        {
            Assert.Equal(HttpStatusCode.BadRequest, this._response?.StatusCode);
        }

        [Fact]
        public async Task Given_A_Payment_To_Process_When_Creating_A_Payment_Then_Returns_Expected_Body()
        {
            // This is testing the shape, not the validity of every single field, done with unit tests
            var body = JsonConvert.DeserializeObject<PostPaymentResponse>(await this._response!.Content
                .ReadAsStringAsync());
            
            Assert.Equal(PaymentStatus.Rejected, body.Status);
        }

        public async Task InitializeAsync()
        {
            var subject = HttpTestSubject.WithExistingPayment(Payments.CreateSavedPayment());
            this._response = await subject.HttpClient.GetAsync($"/api/Payments/ILL_FORMATTED");
        }

        public Task DisposeAsync()
        {
            this._response?.Dispose();
            return Task.CompletedTask;
        }
    }
}