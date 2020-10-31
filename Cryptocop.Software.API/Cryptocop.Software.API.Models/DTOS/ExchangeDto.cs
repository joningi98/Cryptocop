using System;

namespace Cryptocop.Software.API.Models.DTOs
{
    public class ExchangeDto
    {
        public string Id { get; set; }
        public string Exchange_name { get; set; }
        public string Exchange_slug { get; set; }
        public string Base_asset_symbol { get; set; }
        public float? Price_usd { get; set; }
        public DateTime? Last_trade_at { get; set; }
    }
}