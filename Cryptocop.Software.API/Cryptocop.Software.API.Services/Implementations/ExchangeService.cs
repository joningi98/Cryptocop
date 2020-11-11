﻿using System.Collections.Generic;
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
            var requestUri = $"https://data.messari.io/api/v1/markets?page={pageNumber}&fields=id,exchange_id,exchange_name,exchange_slug,base_asset_symbol,price_usd,last_trade_at";
            
            var client = new HttpClient();
            var response = await client.GetAsync(requestUri);

            //TOOD: See if envelople should be used like this
            var envelope = new Envelope<ExchangeDto>
            {
                Items = await response.DeserializeJsonToList<ExchangeDto>(true), PageNumber = pageNumber
            };

            return envelope;

        }
    }
}