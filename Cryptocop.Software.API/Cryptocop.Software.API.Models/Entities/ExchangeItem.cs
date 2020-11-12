using System;

namespace Cryptocop.Software.API.Models.Entities
{
    public class ExchangeItem
    {
        public string exchange_id { get; set; }
        public string exchange_name { get; set; }
        public string exchange_slug { get; set; }
        public string base_asset_symbol { get; set; }
        public float? price_usd { get; set; }
        public DateTime? last_trade_at { get; set; }
    }
}