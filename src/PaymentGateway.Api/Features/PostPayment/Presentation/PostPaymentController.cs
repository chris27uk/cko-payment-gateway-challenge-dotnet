using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Infrastructure;
using PaymentGateway.Api.Shared;

namespace PaymentGateway.Api.Features.PostPayment.Presentation
{
    [Route("api/payments")]
    [ApiController]
    public class PostPaymentController(IPostPaymentHandler createPaymentHandler) : Controller
    {
        [HttpPost]
        public async Task<ActionResult> CreatePaymentAsync(PostPaymentRequest request, CancellationToken cancellationToken = default)
        {
            Activity.Current?.AddTag("Reference", request?.Reference.ToString());
            if (!ModelState.IsValid)
            {
                return BadRequest(request.ToRejectedResponse());
            }
            
            var result = await createPaymentHandler.Handle(request, cancellationToken);
            if (result.Status == PaymentStatus.Rejected)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}