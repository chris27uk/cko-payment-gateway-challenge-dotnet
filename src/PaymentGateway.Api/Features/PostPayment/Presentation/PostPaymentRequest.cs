namespace PaymentGateway.Api.Features.PostPayment.Presentation;

public class PostPaymentRequest
{
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string Currency { get; set; }
    public int Amount { get; set; }
    public int Cvv { get; set; }
    public string CardNumber { get; set; }
}