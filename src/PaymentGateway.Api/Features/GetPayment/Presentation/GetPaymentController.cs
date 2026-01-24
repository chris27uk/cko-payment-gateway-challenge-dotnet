using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Api.Features.GetPayment.Presentation;

[Route("api/Payments")]
[ApiController]
public class GetPaymentController(IGetPaymentHandler paymentHandler) : Controller
{
    [HttpGet("{id}")]
    public ActionResult<GetPaymentResponse?> GetPaymentAsync(Guid id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(GetPaymentResponse.Rejected());
        }
        
        var payment = paymentHandler.Handle(id);
        if (payment == null)
        {
            return NotFound(GetPaymentResponse.Rejected());
        }
        
        return new OkObjectResult(payment);
    }
}