using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Api.Features.GetPayment;
using PaymentGateway.Api.Features.GetPayment.Presentation;
using PaymentGateway.Api.Features.PostPayment;
using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure.Fakes;
using PaymentGateway.Api.Infrastructure.Persistence;
using PaymentGateway.Api.Tests.Infrastructure;

namespace PaymentGateway.Api.Tests.Integration
{
    public class HttpTestSubject : IDisposable
    {
        private readonly WebApplicationFactory<GetPaymentController> _webApplicationFactory;

        private HttpTestSubject(WebApplicationFactory<GetPaymentController> webApplicationFactory, HttpClient httpClient)
        {
            this.HttpClient = httpClient;
            this._webApplicationFactory = webApplicationFactory;
        }

        public HttpClient HttpClient { get; }

        public static HttpTestSubject WithExistingPayment(PostPaymentResponseStored payment, bool useValidationFailure = false)
        {
            var repository = new FakePaymentsRepository(false, false, [payment]);
            var (httpClient, webApplicationFactory) = CreateWebApplicationFactory(repository, useValidationFailure);
            return new HttpTestSubject(webApplicationFactory, httpClient);
        }
        
        public static HttpTestSubject WithNoPriorPayments(bool useValidationFailure = false)
        {
            var (httpClient, webApplicationFactory) = CreateWebApplicationFactory(new FakePaymentsRepository(false,false, []), useValidationFailure);
            return new HttpTestSubject(webApplicationFactory, httpClient);
        }
        
        private static (HttpClient, WebApplicationFactory<GetPaymentController>) CreateWebApplicationFactory(
            FakePaymentsRepository repository,
            bool useValidationFailure)
        {
            var webApplicationFactory = new WebApplicationFactory<GetPaymentController>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("Testing");
                    builder.ConfigureServices(services =>
                    {
                        var serviceCollection = (ServiceCollection)services;
                        serviceCollection.AddSingleton<IPaymentsRepository>(repository);
                        serviceCollection.AddSingleton<IGetPaymentHandler, GetPaymentHandler>();
                        serviceCollection.AddSingleton<IPostPaymentHandler, FakePostPaymentHandler>(_ =>
                            new FakePostPaymentHandler(useValidationFailure));
                        serviceCollection.AddSingleton(repository);
                        serviceCollection.Configure<HttpsRedirectionOptions>(o => o.HttpsPort = 443);
                    });
                });

            var httpClient = webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost"),
                AllowAutoRedirect = false
            });

            httpClient.Timeout = TimeSpan.FromSeconds(2);
            return (httpClient, webApplicationFactory);
        }

        public void Dispose()
        {
            this.HttpClient.Dispose();
            _webApplicationFactory.Dispose();
        }
    }
}