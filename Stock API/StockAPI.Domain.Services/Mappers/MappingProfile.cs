using AutoMapper;
using StockAPI.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Domain.Services.Mappers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Result, Stock>()
                .ForMember(dest => dest.StockTicker, opt => opt.MapFrom(src => src.T))
                .ForMember(dest => dest.ClosestPrice, opt => opt.MapFrom(src => src.c))
                .ForMember(dest => dest.HighestPrice, opt => opt.MapFrom(src => src.h))
                .ForMember(dest => dest.LowestPrice, opt => opt.MapFrom(src => src.l))
                .ForMember(dest => dest.TransactionCount, opt => opt.MapFrom(src => src.n))
                .ForMember(dest => dest.OpenPrice, opt => opt.MapFrom(src => src.o))
                .ForMember(dest => dest.IsOTC, opt => opt.MapFrom(src => src.otc))
                .ForMember(dest => dest.UnixTimestamp, opt => opt.MapFrom(src => src.t))
                .ForMember(dest => dest.TradingVolume, opt => opt.MapFrom(src => src.v))
                .ForMember(dest => dest.VolumeWeightedAveragePrice, opt => opt.MapFrom(src => src.vw));
        }
    }
}
