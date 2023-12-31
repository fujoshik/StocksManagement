﻿using Gateway.Domain.DTOs.Analyzer;

namespace Gateway.Domain.Abstraction.Services
{
    public interface IStatisticsService
    {
        Task<CalculateCurrentYieldDTO> CalculateAverageIncomeAsync(string stockTicker, string date);
        Task<PercentageChangeDTO> GetPercentageChangeAsync(string stockTicker, string date);
        Task<List<DailyYieldChangeDTO>> GetDailyYieldChangesAsync(string date, string stockTicker);
        int GetRequestCount(string route);
        List<string> GetTopUsersByRequests(int count);
        void LogRequest(string route);
    }
}
