namespace Expenses.Models
{
    public class AccountInfoResponse
    {
        public decimal Balance { get; set; }
        public decimal BtcBalance { get; set; }
        public decimal EthBalance { get; set; }
        public decimal LtcBalance { get; set; }
    }
}
