using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class ExchangeService : IExchangeService
    {
        public async Task<Envelope<ExchangeDto>> GetExchanges(int pageNumber = 1)
        {
            var requestUri = $"https://data.messari.io/api/v1/markets?page{pageNumber}&fields=id,exchange_id,exchange_name,exchange_slug,base_asset_symbol,price_usd,last_trade_at";
            HttpClient client = new HttpClient();
           
            HttpResponseMessage response = await client.GetAsync(requestUri);

            var envelope = new Envelope<ExchangeDto>();

            var jslist = await response.DeserializeJsonToList<ExchangeDto>(true);
            System.Console.WriteLine(jslist);

            envelope.Items = jslist;
            envelope.PageNumber = pageNumber;

            return envelope;

        }
    }
}