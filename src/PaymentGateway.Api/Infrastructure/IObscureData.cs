namespace PaymentGateway.Api.Infrastructure
{
    public interface IObscureData
    {
        string Obscure(string value);
    }
}