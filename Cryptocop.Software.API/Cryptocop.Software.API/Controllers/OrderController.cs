using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            /*
            TODO
            Gets all orders associated with the authenticated user
            */

            return Ok();
        }

        [HttpPost]
        public IActionResult CreateOrder()
        {
            /*
            TODO
            Adds a new order associated with the authenticated user, see
            Models section for reference
            */

            return Ok();
        }
    }
}