namespace PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes
{
    public class CustomerReference(Guid value)
    {
        public static implicit operator CustomerReference(Guid value) => new(value);
        
        public Guid Value { get; } = value;
        
        public bool IsValid { get; } = value != Guid.Empty;
    }
}