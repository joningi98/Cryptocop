using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        private readonly IMapper _mapper;

        public CryptoCurrencyService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<CryptocurrencyDto>> GetAvailableCryptocurrencies()
        {
            const string requestUri = "https://data.messari.io/api/v2/assets?fields=id,symbol,name,slug,metrics/market_data/price_usd,profile/general/overview/project_details";
            var client = new HttpClient();
           
            var response = await client.GetAsync(requestUri);
            
            var cryptoList = await response.DeserializeJsonToList<CryptoCurrencyItem>(true);

            var cryptos = new List<string>
            {
                "BTC",
                "ETH",
                "USDT",
                "XMR"
            };
            
            var mappedCryptos = _mapper.Map<IEnumerable<CryptocurrencyDto>>(cryptoList.Where(x => cryptos.Contains(x.symbol)));
            
            return mappedCryptos;
        }
    }
}