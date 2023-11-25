namespace Settlement.Domain.DTOs.Settlement
{
    public class SettlementResponseDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public decimal StockPrice { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
