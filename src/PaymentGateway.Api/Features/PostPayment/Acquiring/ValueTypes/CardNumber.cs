namespace PaymentGateway.Api.Features.PostPayment.Acquiring.ValueTypes
{
    public class CardNumber(string cardNumber)
    {
        public static implicit operator CardNumber(string cardNumber) => new(cardNumber);

        public override string ToString() => cardNumber;

        public int LastFourDigits() => int.Parse(cardNumber[^4..]);
    }
}