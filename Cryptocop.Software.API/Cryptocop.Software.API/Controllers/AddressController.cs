using System.Linq;
using Cryptocop.Software.API.Models.Entities;
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
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

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
        public IActionResult CreateAddress(AddressInputModel addressInput)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "Email").Value;
            _addressService.AddAddress(email, addressInput);
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