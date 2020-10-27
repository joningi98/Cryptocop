using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllAddresses()
        {
            /*
            TODO
            Gets all addresses associated with authenticated user
            */
            return Ok();
        }

        [HttpPost]
        public IActionResult CreateAddress()
        {
            /*
            TODO
            Adds a new address associated with authenticated user, see
            Models section for reference
            */
            return Ok();
        }

        [Route("{id:int}")]
        [HttpDelete]
        public IActionResult DeleteAddress()
        {
            /*
            TODO
            Deletes an address by id 
            */
            return Ok();
        }
    }
}