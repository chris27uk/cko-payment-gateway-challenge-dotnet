using System.Security.Cryptography;
using System.Text;

namespace PaymentGateway.Api.Infrastructure
{
    public class DataObfuscation : IObscureData
    {
        private static readonly string StaticSalt = "A-Secure-Key-Stored-In-Secure-Vault";

        public string Obscure(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value + StaticSalt);
            byte[] hashBytes = SHA256.HashData(bytes);
            return BitConverter.ToString(hashBytes);
        }
    }
}