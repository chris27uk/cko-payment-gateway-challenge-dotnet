using PaymentGateway.Api.Features.GetPayment;
using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure;

namespace PaymentGateway.Api.Features.PostPayment.Idempotency
{
    public class DeduplicationPostPaymentHandler(
        IPostPaymentHandler postPaymentHandler, 
        IGetPaymentHandler getPaymentHandler,  
        IIdempotencyStoreWithTTL idempotencyStore,
        IObservabilityProbe observabilityProbe) : IPostPaymentHandler
    {
        public async Task<PostPaymentResponse> Handle(PostPaymentRequest request)
        {
            if (await idempotencyStore.Add(request.CardNumber!, request.Amount))
            {
                return await postPaymentHandler.Handle(request);
            }

            observabilityProbe.DuplicatePaymentRequest(request.Reference);
            
            var response = getPaymentHandler.Handle(request.Reference);
            return response?.ToPublicPostResponse()!;
        }
    }
}