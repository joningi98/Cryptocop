using System.Linq;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        private string getEmail()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "Email").Value;
            if (email == null) { return null;}
            else { return email; }
        }

        [HttpGet]
        public IActionResult GetCart()
        {
            //Get enmail
            var email = getEmail();

            return Ok(_shoppingCartService.GetCartItems(email));
        }

        [HttpPost]
        public IActionResult AddItemToCart([FromBody] ShoppingCartItemInputModel shoppingCartItem)
        {
            //Get enmail
            var email = getEmail();

            return CreatedAtRoute("", _shoppingCartService.AddCartItem(email, shoppingCartItem));
        }

        [Route("{itemId:int}")]
        public IActionResult DeleteCartItem(int itemId)
        {
            //Get email
            var email = getEmail();
            if (email == null) { return NotFound(); }
            _shoppingCartService.RemoveCartItem(email, itemId);
            return NoContent();
        }

        [Route("{itemId:int}")]
        [HttpPatch]
        public IActionResult UpdateShoppingCartItemQuantity([FromBody] ShoppingCartItemInputModel shoppingCartItemInput, int itemId)
        {
            var quantity = shoppingCartItemInput.Quantity.GetValueOrDefault();
            //Get email
            var email = getEmail();
            if (email == null) { return NotFound(); }
            System.Console.WriteLine(quantity);
            _shoppingCartService.UpdateCartItemQuantity(email, itemId, quantity);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteShoppingChart()
        {
            //Get email
            var email = getEmail();
            if (email == null) { return NotFound(); }
            _shoppingCartService.ClearCart(email);
            return Ok();
        }
    }
}