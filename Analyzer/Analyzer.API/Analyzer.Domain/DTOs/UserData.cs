namespace Analyzer.API.Analyzer.Domain.DTOs
{
    public class UserData
    {
        public Guid AccountGuid { get; set; }

        public decimal Amount { get; set; }

        public decimal CurrentBalance { get; set; }
    }
}
