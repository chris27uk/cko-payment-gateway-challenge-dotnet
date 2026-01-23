using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Features.PostPayment.Presentation;
using PaymentGateway.Api.Infrastructure;

namespace PaymentGateway.Api.Features.GetPayment.Presentation;

[Route("api/Payments")]
[ApiController]
public class GetPaymentController : Controller
{
    private readonly FakePaymentsRepository _paymentsRepository;

    public GetPaymentController(FakePaymentsRepository paymentsRepository)
    {
        _paymentsRepository = paymentsRepository;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostPaymentResponse?>> GetPaymentAsync(Guid id)
    {
        var payment = _paymentsRepository.Get(id);

        if (payment == null)
        {
            return NotFound();
        }
        
        return new OkObjectResult(payment);
    }
}