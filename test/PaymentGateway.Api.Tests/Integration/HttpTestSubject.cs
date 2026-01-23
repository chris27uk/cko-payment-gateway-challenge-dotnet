using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using PaymentGateway.Api.Features.GetPayment;
using PaymentGateway.Api.Features.GetPayment.Contract;
using PaymentGateway.Api.Features.GetPayment.Presentation;
using PaymentGateway.Api.Features.PostPayment.Contract;
using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure;
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

        public static HttpTestSubject WithExistingPayment(PostPaymentResponse payment, bool useValidationFailure = false)
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
            var webApplicationFactory = new WebApplicationFactory<GetPaymentController>();
            var httpClient = webApplicationFactory.WithWebHostBuilder(builder =>
                builder.ConfigureServices(services =>
                {
                    var serviceCollection = (ServiceCollection)services;
                    serviceCollection.AddSingleton<IPaymentsRepository>(repository);
                    serviceCollection.AddSingleton<IGetPaymentHandler, GetPaymentHandler>();
                    serviceCollection.AddSingleton<IPostPaymentHandler, FakePostPaymentHandler>(_ => new FakePostPaymentHandler(useValidationFailure));
                    serviceCollection.AddSingleton(repository);
                })).CreateClient();
            return (httpClient, webApplicationFactory);
        }
        
        public void Dispose() => _webApplicationFactory.Dispose();
    }
}