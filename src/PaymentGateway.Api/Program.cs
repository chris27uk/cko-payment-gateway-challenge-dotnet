using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Features.PostPayment;
using PaymentGateway.Api.Features.PostPayment.Acquiring;
using PaymentGateway.Api.Infrastructure;
using PaymentGateway.Api.Infrastructure.Fakes;
using PaymentGateway.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddSingleton<IAcquiringBankGateway>(new ResilientAcquiringBankGateway(new HttpAcquiringBankGateway()));
builder.Services.AddSingleton<IPostPaymentHandler, PostPaymentHandler>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddSingleton<IObservabilityProbe, FakeObservabilityProbe>();
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
