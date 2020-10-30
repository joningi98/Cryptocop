using Microsoft.AspNetCore.Mvc;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using System.Linq;

namespace Cryptocop.Software.API.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        private string getEmail()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "Email").Value;
            if (email == null) { return null;}
            else { return email; }
        }

        [HttpGet]
        public IActionResult GetAllPaymentCard()
        {
            // Get email
            var email = getEmail();
            if (email == null) { return NotFound(); }

            // Get all cards
            return Ok(_paymentService.GetStoredPaymentCards(email));
        }

        [HttpPost]
        public IActionResult CreatePaymentCard([FromBody] PaymentCardInputModel paymentCard)
        {
            //TODO: If two cardnumbers are the same ?
            // Get email
            var email = getEmail();
            if (email == null) { return NotFound(); }

            System.Console.WriteLine(paymentCard);

            // Crete paymentCard
            _paymentService.AddPaymentCard(email, paymentCard);
            return Ok();
        }
    }
}