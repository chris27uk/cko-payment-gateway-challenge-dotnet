using System.Net;

using Newtonsoft.Json;

using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Shared;

namespace PaymentGateway.Api.Tests.Integration.PostPayment
{
    [Collection("Integration")]

    public class ValidationFailureControllerTests : IAsyncLifetime
    {
        private HttpResponseMessage? _response;
        private HttpTestSubject? _subject;
        
        [Fact]
        public void Given_A_Badly_Formatted_Payment_To_Process_When_Creating_A_Payment_Then_Returns_400()
        {
            Assert.Equal(HttpStatusCode.BadRequest, this._response?.StatusCode);
        }

        [Fact]
        public async Task Given_A_Payment_To_Process_When_Creating_A_Payment_Then_Returns_Valid_Body()
        {
            // This is testing the shape, not the validity of every single field, done with unit tests
            var body = JsonConvert.DeserializeObject<PostPaymentResponse>(await this._response!.Content.ReadAsStringAsync());
            
            Assert.Equal(Guid.Empty, body.Id);
            Assert.Equal(PaymentStatus.Rejected, body.Status);
        }

        private static object CreatePayment()
        {
            return new
            {
                CardNumber = "4444333322221",
                ExpiryMonth = "01",
                ExpiryYear = "2019",
                Cvv = "123",
                Amount = "100",
                Currency = "GBP"
            };
        }

        public async Task InitializeAsync()
        {
            this._subject = HttpTestSubject.WithNoPriorPayments(useValidationFailure: true);
            var json = new StringContent(JsonConvert.SerializeObject(CreatePayment()));
            json.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            this._response = await this._subject.HttpClient.PostAsync($"/api/Payments", json);
        }

        public Task DisposeAsync()
        {
            this._subject?.Dispose();
            this._response?.Dispose();
            return Task.CompletedTask;
        }
    }
}