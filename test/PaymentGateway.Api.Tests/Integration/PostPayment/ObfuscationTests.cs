using PaymentGateway.Api.Infrastructure;

namespace PaymentGateway.Api.Tests.Integration.PostPayment
{
    public class ObfuscationTests
    {
        [Fact]
        public void Given_Data_When_Obfuscating_Then_Returns_Obfuscated_Data()
        {
            var sut = new DataObfuscation();
            string payload = "1234567890";
            var obfuscated = sut.Obscure(payload);
            Assert.NotEqual(payload, obfuscated);
        }
    }
}