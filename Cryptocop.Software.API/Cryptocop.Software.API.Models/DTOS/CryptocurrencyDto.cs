

namespace Cryptocop.Software.API.Models.DTOs
{
    public class CryptocurrencyDto
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Slug { get; set; }
        public float Price_usd { get; set; }
        public string Project_details { get; set; }
    }
}