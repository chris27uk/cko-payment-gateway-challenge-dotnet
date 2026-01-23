using PaymentGateway.Api.Infrastructure;

namespace PaymentGateway.Api.Tests.Infrastructure
{
    public class FakeObfuscation : IObscureData
    {
        public List<string> Requests { get; } = new();

        public string Obscure(string value)
        {
            this.Requests.Add(value);
            return Guid.NewGuid().ToString();
        }
    }
}