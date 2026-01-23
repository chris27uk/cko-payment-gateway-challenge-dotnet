using System.Net;
using System.Net.Http.Json;

using PaymentGateway.Api.Features.PostPayment;
using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Integration.GetPayment
{
    public class GetPaymentControllerTests
    {
        [Fact]
        public async Task Given_No_Payment_When_Requesting_A_Payment_Then_Returns_404()
        {
            var subject = HttpTestSubject.WithNoPriorPayments();
        
            var response = await subject.HttpClient.GetAsync($"/api/Payments/{Guid.NewGuid()}");
        
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Given_Payment_When_Requesting_A_Payment_Then_Returns_200()
        {
            var payment = Payments.CreateSavedPayment();
            var testSubject = HttpTestSubject.WithExistingPayment(payment);
            
            var response = await testSubject.HttpClient.GetAsync($"/api/Payments/{payment.Id}");
            var paymentResponse = await response.Content.ReadFromJsonAsync<PostPaymentResponse>();
        
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(paymentResponse);
        }
    }
}