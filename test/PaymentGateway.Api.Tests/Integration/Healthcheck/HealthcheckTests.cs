using System.Net;

namespace PaymentGateway.Api.Tests.Integration.Healthcheck
{
    [Collection("Integration")]

    public class HealthcheckTests
    {
        [Fact]
        public async Task TestHealthcheck()
        {
            using var subject = HttpTestSubject.WithNoPriorPayments();

            var response = await subject.HttpClient.GetAsync($"/api/Healthcheck");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}