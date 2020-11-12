namespace Cryptocop.Software.API.Models.Entities
{
    public class CryptoCurrencyItem
    {
        public string id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public float price_usd { get; set; }
        public string project_details { get; set; }
    }
}