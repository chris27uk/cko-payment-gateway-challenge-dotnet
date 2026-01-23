using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Api.Features.Healthcheck.Presentation
{
    [Route("api/healthcheck")]
    [ApiController]
    public class HealthcheckController : ControllerBase
    {
        [HttpGet]
        public ActionResult Healthcheck()
        {
            return Ok();
        }
    }
}