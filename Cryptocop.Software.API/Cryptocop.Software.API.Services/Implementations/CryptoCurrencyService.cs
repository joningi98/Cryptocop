using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        public async Task<IEnumerable<CryptocurrencyDto>> GetAvailableCryptocurrencies()
        {
            var requestUri = "https://data.messari.io/api/v2/assets?fields=id,symbol,name,slug,metrics/market_data/price_usd,profile/general/overview/project_details";
            HttpClient client = new HttpClient();
           
            HttpResponseMessage response = await client.GetAsync(requestUri);
            var cryptos = new List<string>
            {
                "BTC",
                "ETH",
                "USDT",
                "XMR"
            };


            return await response.DeserializeJsonToList<CryptocurrencyDto>(true);
;
        }
    }
}