namespace Gateway.Domain.Settings
{
    public class StockApiSettings
    {
        public string GetGroupedDailyRoute { get; set; }
        public string GetStockByDateAndTickerFromAPIRoute { get; set; }
        public string GetStockByDateAndTickerRoute { get; set; }
        public string GetStocksByDateRoute { get; set; }
        public string GetStockMarketCharacteristicsRoute { get; set; }
    }
}
