using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Shared;

namespace PaymentGateway.Api.Infrastructure.Persistence
{
    public class PostPaymentResponseStored
    {
        public Guid Id { get; set; }
        public PaymentStatus Status { get; set; }
        public Guid? AuthorisationCode { get; set; }
        public int CardNumberLastFour { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string? Currency { get; set; }
        public int Amount { get; set; }
    }
}