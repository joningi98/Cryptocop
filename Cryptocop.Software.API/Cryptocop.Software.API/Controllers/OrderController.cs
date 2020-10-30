using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private string getEmail()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "Email").Value;
            if (email == null) { return null;}
            else { return email; }
        }
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            //TODO: Test
            var email = getEmail();
            if (email == null) { return NotFound(); }

            return Ok(_orderService.GetOrders(email));
        }

        [HttpPost]
        public IActionResult CreateOrder(OrderInputModel orderInput)
        {
            //TODO: Test
            // Get email
            var email = getEmail();
            if (email == null) { return NotFound(); }

            // Create order 
            return CreatedAtRoute("", _orderService.CreateNewOrder(email, orderInput));
        }
    }
}