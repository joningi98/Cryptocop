using Cryptocop.Software.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterInputModel register)
        {
            /*
            TODO: CreateUser
            Registers a user within the application, see Models
            section for reference
            */
            return CreatedAtRoute("TODO", register);
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
            return Ok();
        }

        [HttpGet]
        [Route("signout")]
        public IActionResult SignOut()
        {
            /*
            TODO: Logout
             Logs the user out by voiding the provided JWT token
             using the id found within the claim
            */
            return NoContent();
        }       
    }
}