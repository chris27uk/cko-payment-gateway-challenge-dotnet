namespace PaymentGateway.Api.Features.PostPayment.Acquiring
{
    public class AcquiringBankTransientError(Exception innerException) : Exception("Transient error during acquiring.",
        innerException);
}