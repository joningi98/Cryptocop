using AutoMapper;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Cryptocop.Software.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CryptoCurrencyItem, CryptocurrencyDto>()
                .ForMember(
                    src => src.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(src => src.Symbol, opt => opt.MapFrom(src => src.symbol))
                .ForMember(src => src.Name, opt => opt.MapFrom(src => src.name))
                .ForMember(src => src.Slug, opt => opt.MapFrom(src => src.slug))
                .ForMember(src => src.PriceInUsd, opt => opt.MapFrom(src => src.price_usd))
                .ForMember(src => src.ProjectDetails, opt => opt.MapFrom(src => src.project_details));

            CreateMap<ExchangeItem, ExchangeDto>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.exchange_id))
                .ForMember(src => src.Name, opt => opt.MapFrom(src => src.exchange_name))
                .ForMember(src => src.Slug, opt => opt.MapFrom(src => src.exchange_slug))
                .ForMember(src => src.AssetSymbol, opt => opt.MapFrom(src => src.base_asset_symbol))
                .ForMember(src => src.PriceInUsd, opt => opt.MapFrom(src => src.price_usd))
                .ForMember(src => src.LastTrade, opt => opt.MapFrom(src => src.last_trade_at));
        }
    }
}