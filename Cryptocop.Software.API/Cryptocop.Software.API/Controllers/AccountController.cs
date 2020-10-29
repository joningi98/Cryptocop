using System.Linq;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterInputModel register)
        {
            var user = _accountService.CreateUser(register);

            // If user already exists 
            if (user == null) { return StatusCode(401); }

            // Return the user
            return CreatedAtRoute("", user);
        }

        [HttpPost]
        [Route("signin")]
        public IActionResult SignIn([FromBody] LoginInputModel login)
        {
            /*
            TODO: AuthenticateUser
            Signs the user in by checking the credentials provided
            and issuing a JWT token in return, see Models section for reference
            */
            var user = _accountService.AuthenticateUser(login);
            if (user == null) { return Unauthorized(); }
            var token = _tokenService.GenerateJwtToken(user);
            return Ok(token);
        }

        [Authorize]
        [HttpGet]
        [Route("signout")]
        public IActionResult SignOut()
        {
            /*
            TODO: Logout
             Logs the user out by voiding the provided JWT token
             using the id found within the claim
            */
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "tokenId").Value, out var tokenId);
            _accountService.Logout(tokenId);
            return NoContent();
        }       
    }
}