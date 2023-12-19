namespace Settlement.Domain.DTOs.Handled
{
    public class HandledWalletsDto : BaseDto
    {
        public Guid WalletId { get; set; }
        public Guid AccountId { get; set; }
        public Guid TransactionId { get; set; }
    }
}
