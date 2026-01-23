using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Features.PostPayment.Contract;
using PaymentGateway.Api.Shared;

namespace PaymentGateway.Api.Features.PostPayment.Presentation
{
    [Route("api/payments")]
    [ApiController]
    public class CreatePaymentController(IPostPaymentHandler createPaymentHandler) : Controller
    {
        [HttpPost]
        public ActionResult CreatePaymentAsync(PostPaymentRequest request)
        {
            return Ok(createPaymentHandler.Handle(request));
        }
    }
}