using Cryptocop.Software.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        // TODO: Setup routes
        [HttpGet]
        public IActionResult GetCart()
        {
            /*
            TODO
            Gets all items within the shopping cart, see Models section for reference
            */
            return Ok();
        }

        [HttpPost]
        public IActionResult AddItemToCart([FromBody] ShoppingCartItemInputModel shoppingCartItem)
        {
            /*
            TODO
            Adds an item to the shopping cart, see Models section for reference
            */
            return Ok();
        }

        [Route("{id:int}")]
        public IActionResult DeleteCartItem()
        {
            /*
            TODO
            Deletes an item from the shopping cart
            */
            return Ok();
        }

        [Route("{id:int}")]
        [HttpPatch]
        public IActionResult UpdateShoppingCartItemQuantity()
        {
            /*
            TODO
            Updates the quantity for a shopping cart item
            */
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteShoppingChart()
        {
            /*
            TODO
            Clears the cart - all items within the cart should be deleted
            */
            return Ok();
        }
    }
}