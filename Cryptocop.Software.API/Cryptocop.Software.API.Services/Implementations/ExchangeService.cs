using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class ExchangeService : IExchangeService
    {
        private readonly IMapper _mapper;

        public ExchangeService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<Envelope<ExchangeDto>> GetExchanges(int pageNumber = 1)
        {
            var requestUri = $"https://data.messari.io/api/v1/markets?page={pageNumber}&fields=id,exchange_id,exchange_name,exchange_slug,base_asset_symbol,price_usd,last_trade_at";
            
            var client = new HttpClient();
            var response = await client.GetAsync(requestUri);

            var exchangeList = await response.DeserializeJsonToList<ExchangeItem>(true);
            var envelope = new Envelope<ExchangeDto>
            {
                Items = _mapper.Map<IEnumerable<ExchangeDto>>(exchangeList),
                PageNumber = pageNumber
            };

            return envelope;
            
        }
    }
}