namespace PaymentGateway.Api.Features.PostPayment.Acquiring
{
    public class AuthorisationResponse
    {
        private AuthorisationResponse(Guid? authorisationCode, bool authorised)
        {
            AuthorisationCode = authorisationCode;
            Authorised = authorised;
        }

        public Guid? AuthorisationCode { get; }
        
        public bool Authorised { get; }
        
        public static AuthorisationResponse ForAuthorised(Guid authorisationCode)
        {
            return new AuthorisationResponse(authorisationCode, true);
        }
        
        public static AuthorisationResponse ForRejected()
        {
            return new AuthorisationResponse(null, false);
        }
    }
}