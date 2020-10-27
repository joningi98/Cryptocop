using System;

namespace Cryptocop.Software.API.Models.DTOs
{
    public class ExchangeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string AssertSymbol { get; set; }
        public float? PriceUsd { get; set; }
        public DateTime? LastTrade { get; set; }
    }
}