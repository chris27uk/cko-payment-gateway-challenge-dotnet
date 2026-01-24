using System.Net;

using Newtonsoft.Json;

using PaymentGateway.Api.Features.GetPayment.Presentation;
using PaymentGateway.Api.Shared;

namespace PaymentGateway.Api.Tests.Integration.GetPayment
{
    [Collection("Integration")]
    public class NotFoundControllerTests : IAsyncLifetime
    {
        private HttpResponseMessage? _response;
        private HttpTestSubject? _subject;

        [Fact]
        public void Given_No_Payment_When_Requesting_A_Payment_Then_Returns_404()
        {
            Assert.Equal(HttpStatusCode.NotFound, this._response?.StatusCode);
        }
        
        [Fact]
        public async Task Given_No_Payment_When_Requesting_A_Payment_Then_Returns_Rejected_In_Body()
        {
            // It is important for the consumer to be able to differentiate between an infrastructure not found
            // due to a badly constructed URI or the data non-existence of a valid call. Many third party load
            // balancers, and proxies like the Traefik proxy used in major cloud platforms return 404 for
            // infrastructure reasons. The only way to differentiate is to return data in the body,
            // the specification indicates we should only return bodies that conform. 
            var deserialized = JsonConvert.DeserializeObject<GetPaymentResponse>(await _response!.Content.ReadAsStringAsync());
            Assert.Equal(PaymentStatus.Rejected, deserialized.Status);
        }
        
        public async Task InitializeAsync()
        {
            this._subject = HttpTestSubject.WithNoPriorPayments();
            this._response = await this._subject.HttpClient.GetAsync($"/api/Payments/{Guid.NewGuid()}");
        }

        public Task DisposeAsync()
        {
            this._subject?.Dispose();
            this._response?.Dispose();
            return Task.CompletedTask;
        }
    }
}