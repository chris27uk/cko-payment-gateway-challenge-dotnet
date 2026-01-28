using System.Text.Json.Serialization;

using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc;

using OpenTelemetry.Logs;
using OpenTelemetry.Trace;

using PaymentGateway.Api.Features.GetPayment;
using PaymentGateway.Api.Features.PostPayment;
using PaymentGateway.Api.Features.PostPayment.Acquiring;
using PaymentGateway.Api.Features.PostPayment.Idempotency;
using PaymentGateway.Api.Infrastructure;
using PaymentGateway.Api.Infrastructure.Fakes;
using PaymentGateway.Api.Infrastructure.Observability;
using PaymentGateway.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddApplicationInsightsTelemetry(options => {
        options.ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint=http://localhost:8080/";
    });
}

builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddOpenTelemetry()
    .WithLogging(logging => logging.AddConsoleExporter())
    .WithTracing(logging => logging.AddConsoleExporter());

builder.Services
    .AddSingleton<IAcquiringBankGateway>(sp => new ResilientAcquiringBankGateway(new HttpAcquiringBankGateway(sp.GetRequiredService<HttpClient>())))
    .AddHttpClient<IAcquiringBankGateway, HttpAcquiringBankGateway>(c =>
    {
        c.BaseAddress = new Uri("http://localhost:8080");
        c.Timeout = TimeSpan.FromSeconds(2);
    })
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        // Force connection refresh every 2 minutes to respect DNS changes
        PooledConnectionLifetime = TimeSpan.FromMinutes(2) 
    })
    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

builder.Services.AddSingleton<IIdempotencyStoreWithTTL, FakeIdempotencyRepository>();
builder.Services.AddSingleton<IPostPaymentHandler, DeduplicationPostPaymentHandler>(GetPostPaymentHandler);
builder.Services.AddSingleton<IGetPaymentHandler, GetPaymentHandler>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddSingleton<IObservabilityProbe, ApplicationInsightsTelemetryProbe>();
builder.Services.AddSingleton<IPaymentsRepository>(_ => new ResilientPaymentsRepository(new FakePaymentsRepository(false, false, [])));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

DeduplicationPostPaymentHandler GetPostPaymentHandler(IServiceProvider sp)
{
    var postPaymentHandler = new PostPaymentHandler(
        sp.GetRequiredService<IPaymentsRepository>(),
        sp.GetRequiredService<IAcquiringBankGateway>(), sp.GetRequiredService<IDateTimeProvider>(),
        sp.GetRequiredService<IObservabilityProbe>());
    return new DeduplicationPostPaymentHandler(
        postPaymentHandler, sp.GetRequiredService<IGetPaymentHandler>(),
        sp.GetRequiredService<IIdempotencyStoreWithTTL>(),
        sp.GetRequiredService<IObservabilityProbe>());
}
