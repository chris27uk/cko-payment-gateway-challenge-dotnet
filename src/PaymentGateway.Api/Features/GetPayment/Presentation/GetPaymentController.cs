using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Infrastructure;
using PaymentGateway.Api.Shared;

namespace PaymentGateway.Api.Features.GetPayment.Presentation;

[Route("api/Payments")]
[ApiController]
public class GetPaymentController(FakePaymentsRepository paymentsRepository) : Controller
{
    [HttpGet("{id}")]
    public async Task<ActionResult<GetPaymentResponse?>> GetPaymentAsync(Guid id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new GetPaymentResponse
            {
                Status = PaymentStatus.Rejected
            });
        }
        
        var payment = paymentsRepository.Get(id);

        if (payment == null)
        {
            return NotFound(new GetPaymentResponse { Status = PaymentStatus.Rejected });
        }
        
        return new OkObjectResult(payment);
    }
}