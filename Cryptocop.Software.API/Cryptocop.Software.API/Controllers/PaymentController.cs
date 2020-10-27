using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllPaymentCard()
        {
            /*
            TODO
            Gets all payment cards associated with the authenticated user
            */
            
            return Ok();
        }

        [HttpPost]
        public IActionResult CreatePaymentCard()
        {
            /*
            TODO
            Adds a new payment card associated with the authenticated
            user, see Models section for reference
            */

            return Ok();
        }
    }
}