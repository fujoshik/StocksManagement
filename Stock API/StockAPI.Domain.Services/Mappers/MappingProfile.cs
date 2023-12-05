using AutoMapper;
using StockAPI.Infrastructure.Models;
using StockAPI.Infrastructure.Models.FillData;
using StockAPI.Infrastructure.Models.GetGroupedDaily;
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

            CreateMap<StockByDateAndTickerRoot, Stock>()
                .ForMember(dest => dest.StockTicker, opt => opt.MapFrom(src => src.symbol))
                .ForMember(dest => dest.ClosestPrice, opt => opt.MapFrom(src => src.close))
                .ForMember(dest => dest.HighestPrice, opt => opt.MapFrom(src => src.high))
                .ForMember(dest => dest.LowestPrice, opt => opt.MapFrom(src => src.low))
                .ForMember(dest => dest.OpenPrice, opt => opt.MapFrom(src => src.open))
                .ForMember(dest => dest.TradingVolume, opt => opt.MapFrom(src => src.volume))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.from));

            CreateMap<TimeSeries, Stock>()
            //.ForMember(dest => dest.StockTicker, opt => opt.MapFrom(src => result.MetaData.Symbol))
            .ForMember(dest => dest.ClosestPrice, opt => opt.MapFrom(src => src.Close))
            .ForMember(dest => dest.HighestPrice, opt => opt.MapFrom(src => src.High))
            .ForMember(dest => dest.LowestPrice, opt => opt.MapFrom(src => src.Low))
            .ForMember(dest => dest.TransactionCount, opt => opt.MapFrom(src => Convert.ToInt32(src.Volume)))
            .ForMember(dest => dest.OpenPrice, opt => opt.MapFrom(src => Convert.ToDouble(src.Open)))
            .ForMember(dest => dest.IsOTC, opt => opt.MapFrom(src => false))
            //.ForMember(dest => dest.UnixTimestamp, opt => opt.MapFrom(src => DateTimeOffset.Parse(src.Date).ToUnixTimeSeconds()))
            .ForMember(dest => dest.TradingVolume, opt => opt.MapFrom(src => Convert.ToDouble(src.Volume)));
            //.ForMember(dest => dest.VolumeWeightedAveragePrice, opt => opt.MapFrom(src => null))
            //.ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));

        }
    }
}
