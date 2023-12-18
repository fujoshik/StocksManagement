namespace Gateway.Domain.Settings
{
    public class AccountSettings
    {
        public string RegisterTrialRoute { get; set; }
        public string RegisterRoute { get; set; }
        public string LoginRoute { get; set; }
        public string VerifyCodeRoute { get; set; }
        public string DepositRoute { get; set; }
        public string ChangeCurrencyRoute { get; set; }
        public string GetWalletInfoRoute { get; set; }
        public string BuyStockRoute { get; set; }
        public string SellStockRoute { get; set; }
        public string CalculateAverageIncomeRoute { get; set; }
    }
}
