using System.Net;
using System.Text.Json.Serialization;

using PaymentGateway.Api.Features.PostPayment.Acquiring;

namespace PaymentGateway.Api.Infrastructure
{
    public class HttpAcquiringBankGateway(HttpClient client) : IAcquiringBankGateway
    {
        public async Task<AuthorisationResponse> AuthorisePayment(AuthorisationRequest request)
        {
            try
            {
                var response = await client.PostAsJsonAsync("/payments", ToDto(request));
                response.EnsureSuccessStatusCode();
                var deserializedResponse = await response.Content.ReadFromJsonAsync<AuthorisationResponseDto>();
                return deserializedResponse is { Authorised: true } ? AuthorisationResponse.ForAuthorised(new Guid(deserializedResponse.AuthorisationCode!)) : AuthorisationResponse.ForDeclined();
            }
            catch (HttpRequestException hre) when (hre.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new AcquiringBankTransientError(hre);
            }
        }

        private static AuthorisationRequestDto ToDto(AuthorisationRequest request)
        {
            return new AuthorisationRequestDto
            {
                CardNumber = request.CardNumber.ToString(),
                ExpiryDate = request.ExpiryDate.ToString(),
                Currency = request.Currency.ToString(),
                Amount = request.Amount.Value,
                Cvv = request.Cvv.Value
            };
        }

        private class AuthorisationResponseDto
        {
            [JsonPropertyName("authorized")]
            public bool Authorised { get; set; }
            
            [JsonPropertyName("authorization_code")]
            public string? AuthorisationCode { get; set; }
        }
        
        private class AuthorisationRequestDto
        {
            [JsonPropertyName("card_number")]
            public string? CardNumber { get; set; }
            
            [JsonPropertyName("expiry_date")]
            public string? ExpiryDate { get; set; }
            
            [JsonPropertyName("currency")]
            public string? Currency { get; set; }
            
            [JsonPropertyName("amount")]
            public int Amount { get; set; }
            
            [JsonPropertyName("cvv")]
            public string? Cvv { get; set; }
        }
    }
}