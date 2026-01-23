using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Tests.Integration
{
    public class HttpTestSubject : IDisposable
    {
        private readonly WebApplicationFactory<PaymentsController> _webApplicationFactory;

        private HttpTestSubject(WebApplicationFactory<PaymentsController> webApplicationFactory, HttpClient httpClient)
        {
            this.HttpClient = httpClient;
            this._webApplicationFactory = webApplicationFactory;
        }

        public HttpClient HttpClient { get; }

        public static HttpTestSubject WithExistingPayment(PostPaymentResponse payment, bool useValidationFailure = false)
        {
            var repository = new PaymentsRepository();
            repository.Add(payment);
            
            var (httpClient, webApplicationFactory) = CreateWebApplicationFactory(repository, useValidationFailure);
            return new HttpTestSubject(webApplicationFactory, httpClient);
        }
        
        public static HttpTestSubject WithNoPriorPayments(bool useValidationFailure = false)
        {
            var (httpClient, webApplicationFactory) = CreateWebApplicationFactory(new PaymentsRepository(), useValidationFailure);
            return new HttpTestSubject(webApplicationFactory, httpClient);
        }
        
        private static (HttpClient, WebApplicationFactory<PaymentsController>) CreateWebApplicationFactory(
            PaymentsRepository repository,
            bool useValidationFailure)
        {
            var webApplicationFactory = new WebApplicationFactory<PaymentsController>();
            var httpClient = webApplicationFactory.WithWebHostBuilder(builder =>
                builder.ConfigureServices(services =>
                {
                    var serviceCollection = (ServiceCollection)services;
                    serviceCollection.AddSingleton(repository);
                })).CreateClient();
            return (httpClient, webApplicationFactory);
        }
        
        public void Dispose() => _webApplicationFactory.Dispose();
    }
}