using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure;

namespace PaymentGateway.Api.Features.PostPayment.Idempotency
{
    public class DeduplicationPostPaymentHandler(IPostPaymentHandler postPaymentHandler, IIdempotencyStoreWithTTL idempotencyStore) : IPostPaymentHandler
    {
        public async Task<PostPaymentResponse> Handle(PostPaymentRequest request)
        {
            if (await idempotencyStore.Add(request.CardNumber!, request.Amount))
            {
                return await postPaymentHandler.Handle(request);
            }

            return request.ToRejectedResponse();
        }
    }
}