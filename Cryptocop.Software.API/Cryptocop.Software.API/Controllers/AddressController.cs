using System.Linq;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private string getEmail()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "Email").Value;
            if (email == null) { return null;}
            else { return email; }
        }
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public IActionResult GetAllAddresses()
        {
            var email = getEmail();
            if (email == null) { return NotFound(); }
            return Ok(_addressService.GetAllAddresses(email));
        }

        [HttpPost]
        public IActionResult CreateAddress(AddressInputModel addressInput)
        {
            //TODO: Assume the input is correct ? 
            var email = getEmail();
            if (email == null ) { return NotFound(); }
            _addressService.AddAddress(email, addressInput);
            return Ok();
        }

        [Route("{addressId:int}")]
        [HttpDelete]
        public IActionResult DeleteAddress(int addressId)
        {
            //TOOD: ID address does not exist?
            var email = getEmail();
            if (email == null ) { return NotFound(); }
            _addressService.DeleteAddress(email, addressId);
            return Ok();
        }
    }
}