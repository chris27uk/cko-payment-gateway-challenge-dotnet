using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Features.PostPayment.Contract;
using PaymentGateway.Api.Infrastructure;
using PaymentGateway.Api.Shared;

namespace PaymentGateway.Api.Features.PostPayment.Presentation
{
    [Route("api/payments")]
    [ApiController]
    public class PostPaymentController(IPostPaymentHandler createPaymentHandler) : Controller
    {
        [HttpPost]
        public ActionResult CreatePaymentAsync(PostPaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request.ToRejectedResponse());
            }
            
            var result = createPaymentHandler.Handle(request);
            if (result.Status == PaymentStatus.Rejected)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}