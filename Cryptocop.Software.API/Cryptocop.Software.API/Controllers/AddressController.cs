﻿using System.Linq;
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
        private string GetEmail()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            return email ?? null;
        }
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public IActionResult GetAllAddresses()
        {
            var email = GetEmail();
            if (email == null) { return NotFound(); }
            return Ok(_addressService.GetAllAddresses(email));
        }
 
        [HttpPost]
        public IActionResult CreateAddress(AddressInputModel addressInput)
        {
            if (!ModelState.IsValid) { return BadRequest(); }
            var email = GetEmail();
            if (email == null ) { return NotFound(); }
            _addressService.AddAddress(email, addressInput);
            return CreatedAtRoute("", null);
        }

        [Route("{addressId:int}")]
        [HttpDelete]
        public IActionResult DeleteAddress(int addressId)
        {
            var email = GetEmail();
            if (email == null ) { return NotFound(); }
            _addressService.DeleteAddress(email, addressId);
            return NoContent();
        }
    }
}