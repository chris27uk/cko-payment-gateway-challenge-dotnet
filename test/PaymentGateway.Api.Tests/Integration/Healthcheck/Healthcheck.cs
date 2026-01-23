using System.Net;

namespace PaymentGateway.Api.Tests.Integration.Healthcheck
{
    public class HealthcheckTests
    {
        [Fact]
        public async Task TestHealthcheck()
        {
            var subject = HttpTestSubject.WithNoPriorPayments();

            var response = await subject.HttpClient.GetAsync($"/api/Healthcheck");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}