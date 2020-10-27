using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [ApiController]
    [Route("api/cryptocurrencies")]
    public class CryptoCurrencyController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllCryptocurrencies()
        {
            /*
            TODO
            Gets all available cryptocurrencies - the only available
            cryptocurrencies in this platform are BitCoin (BTC), Ethereum (ETH), Tether (USDT) and
            Monero (XMR)
            */
            return Ok();
        }
    }
}
