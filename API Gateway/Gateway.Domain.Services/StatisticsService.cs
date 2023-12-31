﻿using Gateway.Domain.Abstraction.Factories;
using Gateway.Domain.Abstraction.Services;
using Gateway.Domain.DTOs.Analyzer;

namespace Gateway.Domain.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly Dictionary<string, int> _requestCounts = new Dictionary<string, int>();
        private readonly Dictionary<string, List<string>> _userRequests = new Dictionary<string, List<string>>();
        private readonly IHttpClientFactoryCustom _httpClientFactoryCustom;

        public StatisticsService(IHttpClientFactoryCustom httpClientFactoryCustom)
        {
            _httpClientFactoryCustom = httpClientFactoryCustom;
        }

        public async Task<CalculateCurrentYieldDTO> CalculateAverageIncomeAsync(string stockTicker, string date)
        {
            return await _httpClientFactoryCustom
                .GetAccountClient()
                .GetStockAccountClient()
                .CalculateAverageIncomeAsync(stockTicker, date);
        }

        public async Task<PercentageChangeDTO> GetPercentageChangeAsync(string stockTicker, string date)
        {
            return await _httpClientFactoryCustom
                .GetAccountClient()
                .GetStockAccountClient()
                .GetPercentageChangeAsync(stockTicker, date);
        }

        public async Task<List<DailyYieldChangeDTO>> GetDailyYieldChangesAsync(string date, string stockTicker)
        {
            return await _httpClientFactoryCustom
                .GetAccountClient()
                .GetStockAccountClient()
                .GetDailyYieldChangesAsync(stockTicker, date);
        }

        public int GetRequestCount(string route)
        {
            return _requestCounts.TryGetValue(route, out int count) ? count : 0;
        }

        public List<string> GetTopUsersByRequests(int count)
        {
            return _userRequests.OrderByDescending(kv => kv.Value.Count)
                                .Take(count)
                                .Select(kv => kv.Key)
                                .ToList();
        }

        public void LogRequest(string userId, string route)
        {
            
            if (_requestCounts.ContainsKey(route))
                _requestCounts[route]++;
            else
                _requestCounts[route] = 1;

            
            if (_userRequests.ContainsKey(userId))
                _userRequests[userId].Add(route);
            else
                _userRequests[userId] = new List<string> { route };

            //services.AddSingleton<IStatisticsService, StatisticsService>();
        }

        public void LogRequest(string route)
        {
            throw new NotImplementedException();
        }
    }

}
