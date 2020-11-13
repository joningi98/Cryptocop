using Microsoft.AspNetCore.Mvc;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        private string GetEmail()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "Email").Value;
            return email;
        }

        [HttpGet]
        public IActionResult GetAllPaymentCard()
        {
            // Get email
            var email = GetEmail();
            if (email == null) { return NotFound(); }

            // Get all cards
            return Ok(_paymentService.GetStoredPaymentCards(email));
        }

        [HttpPost]
        public IActionResult CreatePaymentCard([FromBody] PaymentCardInputModel paymentCard)
        {
            if (!ModelState.IsValid) { return BadRequest("Input state wrong"); }
            // Get email
            var email = GetEmail();
            if (email == null) { return NotFound(); }

            System.Console.WriteLine(paymentCard);

            // Crete paymentCard
            _paymentService.AddPaymentCard(email, paymentCard);
            return CreatedAtRoute("",null);
        }
    }
}